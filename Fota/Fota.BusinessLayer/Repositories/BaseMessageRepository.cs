


using Microsoft.EntityFrameworkCore;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Fota.Models;
using Fota.Services;
using Microsoft.Extensions.Logging;
using Fota.DataLayer.Enum;
namespace Fota.BusinessLayer.Repositories
{
    public class BaseMessageRepository : GenericRepository<BaseMessage>, IBaseMessageRepository
    {
        // ✅ استخدام Logger خاص بالـ Repository
        private readonly ILogger<BaseMessageRepository> _logger;
        private readonly MqttService _mqttService;
        private readonly FOTADbContext _context;

        public BaseMessageRepository(
            FOTADbContext context,
            ILogger<BaseMessageRepository> logger,
            MqttService mqttService) : base(context)
        {
            _logger = logger;
            _mqttService = mqttService;
            _context = context;
        }

        public async Task<IEnumerable<BaseMessage>> GetAllAsync()
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Include(m => m.Uploader)
                .Include(m => m.Publisher)
                .Include(m => m.Deliveries)
                .Where(m => !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<BaseMessage?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Include(m => m.Uploader)
                .Include(m => m.Publisher)
                .Include(m => m.Deliveries)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<IEnumerable<BaseMessage>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(m => m.HexFileName.Contains(name) && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseMessage>> GetByStatusAsync(BaseMessageStatus status)
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Include(m => m.Uploader)
                .Where(m => m.Status == status && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseMessage>> GetByMessageTypeAsync(BaseMessageType messageType)
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Include(m => m.Uploader)
                .Where(m => m.MessageType == messageType && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseMessage>> GetByUploaderAsync(int uploaderId)
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Where(m => m.UploaderId == uploaderId && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseMessage>> GetByTopicAsync(int topicId)
        {
            return await _dbSet
                .Include(m => m.Uploader)
                .Include(m => m.Publisher)
                .Where(m => m.TopicId == topicId && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseMessage>> GetPublishedMessagesAsync()
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Include(m => m.Uploader)
                .Include(m => m.Publisher)
                .Where(m => m.Status == BaseMessageStatus.Published && !m.IsDeleted)
                .OrderByDescending(m => m.PublishedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseMessage>> GetPendingApprovalAsync()
        {
            return await _dbSet
                .Include(m => m.Topic)
                .Include(m => m.Uploader)
                .Where(m => m.Status == BaseMessageStatus.Pending && !m.IsDeleted)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<BaseMessage?> ApproveMessageAsync(int messageId, int approvedById)
        {
            var message = await GetByIdAsync(messageId);
            if (message == null || message.Status != BaseMessageStatus.Pending)
                return null;

            message.Status = BaseMessageStatus.Approved;
            message.ApprovedById = approvedById;
            message.ApprovedAt = DateTime.UtcNow;
            message.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<BaseMessage?> RejectMessageAsync(int messageId, string rejectionReason)
        {
            var message = await GetByIdAsync(messageId);
            if (message == null || message.Status != BaseMessageStatus.Pending)
                return null;

            message.Status = BaseMessageStatus.Rejected;
            message.RejectedAt = DateTime.UtcNow;
            message.RejectionReason = rejectionReason;
            message.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<BaseMessage?> PublishMessageAsync(int messageId, int publisherId)
        {
            _logger.LogInformation("🔄 Starting publish process for message {MessageId}", messageId);

            var message = await _context.BaseMessages
                .Include(m => m.Topic)
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message == null)
            {
                _logger.LogWarning("❌ Message {MessageId} not found", messageId);
                return null;
            }

            if (message.Status != BaseMessageStatus.Approved)
            {
                _logger.LogWarning("❌ Message {MessageId} status is {Status}, must be Approved",
                    messageId, message.Status);
                return null;
            }

            if (message.Topic == null)
            {
                _logger.LogError("❌ Message {MessageId} has no associated topic", messageId);
                return null;
            }

            // ✅ تحقق إن الـ HexFileContent مش فاضي (هو دلوقتي string)
            if (string.IsNullOrEmpty(message.HexFileContent))
            {
                _logger.LogError("❌ Message {MessageId} has no hex file content", messageId);
                return null;
            }

            try
            {
                // ✅ الـ HexFileContent دلوقتي string (Base64) مش محتاج Convert
                _logger.LogInformation(
                    "📤 Publishing message {MessageId} to MQTT topic '{TopicName}' - Content: {Base64Length} Base64 chars",
                    messageId, message.Topic.Name, message.HexFileContent.Length);

                // ✅ Check MQTT connection status
                if (!_mqttService.IsConnected)
                {
                    _logger.LogWarning("⚠️ MQTT not connected. Attempting to connect...");
                    await _mqttService.ConnectAsync();
                }

                // ✅ Publish to MQTT (الـ HexFileContent هو Base64 string جاهز)
                await _mqttService.PublishAsync(message.Topic.Name, message.HexFileContent);

                _logger.LogInformation(
                    "✅ Successfully published message {MessageId} to MQTT topic '{TopicName}'",
                    messageId, message.Topic.Name);

                // Update message status in database
                message.Status = BaseMessageStatus.Published;
                message.PublisherId = publisherId;
                message.PublishedAt = DateTime.UtcNow;
                message.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Database updated - Message {MessageId} status: Published", messageId);

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to publish message {MessageId} to MQTT", messageId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await GetByIdAsync(id);
            if (message == null) return false;

            message.IsDeleted = true;
            message.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<BaseMessage> AddAsync(BaseMessage entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BaseMessage> UpdateAsync(BaseMessage entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}