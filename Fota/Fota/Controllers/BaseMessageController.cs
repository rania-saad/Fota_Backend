

//using AutoMapper;
//using Fota.BusinessLayer.Interfaces;
//using Fota.BusinessLayer.Repositories;
//using Fota.DataLayer.Enum;
//using Fota.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SharedProjectDTOs.BaseMessages;
//using System.Security.Claims;

//namespace StaffAffairs.AWebAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BaseMessagesController : ControllerBase
//    {
//        private readonly IBaseMessageRepository _baseMessageRepository;
//        private readonly IDeveloperRepository _developerRepository;
//        private readonly IMapper _mapper;
//        // ✅ استخدام Logger خاص بالـ Controller
//        private readonly ILogger<BaseMessagesController> _logger;

//        public BaseMessagesController(
//            IBaseMessageRepository baseMessageRepository,
//            IMapper mapper,
//            IDeveloperRepository developerRepository,
//        ILogger<BaseMessagesController> logger)
//        {
//            _baseMessageRepository = baseMessageRepository;
//            _mapper = mapper;
//            _logger = logger;
//            _developerRepository=developerRepository;
//        }

//        private async Task<int> GetCurrentDeveloperIdAsync()
//        {
//            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(identityUserId))
//                throw new UnauthorizedAccessException("User is not authenticated");

//            var developerId = await _developerRepository
//                .GetDeveloperIdByIdentityUserIdAsync(identityUserId);

//            if (!developerId.HasValue)
//                throw new UnauthorizedAccessException("Developer not found for this user");

//            return developerId.Value;
//        }






//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetAllBaseMessages()
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetAllAsync();
//                if (messages == null || !messages.Any())
//                    return NoContent();

//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting all base messages");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }


//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpGet("{id}")]
//        public async Task<ActionResult<BaseMessageGetDto>> GetBaseMessageById(int id)
//        {
//            try
//            {
//                var message = await _baseMessageRepository.GetByIdAsync(id);
//                if (message == null)
//                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });

//                var messageDto = _mapper.Map<BaseMessageGetDto>(message);
//                return Ok(messageDto);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting base message {Id}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpGet("{id}/file")]
//        public async Task<ActionResult<BaseMessageWithFileDto>> GetBaseMessageWithFile(int id)
//        {
//            try
//            {
//                var message = await _baseMessageRepository.GetByIdAsync(id);
//                if (message == null)
//                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });

//                var messageDto = _mapper.Map<BaseMessageWithFileDto>(message);
//                return Ok(messageDto);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting base message with file {Id}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]

//        [HttpGet("search")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> SearchBaseMessages([FromQuery] string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//                return BadRequest(new { message = "Search name cannot be empty" });

//            try
//            {
//                var messages = await _baseMessageRepository.SearchByNameAsync(name);
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error searching base messages");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpGet("status/{status}")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByStatus(BaseMessageStatus status)
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetByStatusAsync(status);
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting messages by status {Status}", status);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]

//        [HttpGet("type/{messageType}")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByType(BaseMessageType messageType)
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetByMessageTypeAsync(messageType);
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting messages by type {MessageType}", messageType);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }
//        [Authorize(Roles = "ADMIN")]
//        [HttpGet("uploader/{uploaderId}")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByUploader(int uploaderId)
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetByUploaderAsync(uploaderId);
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting messages by uploader {UploaderId}", uploaderId);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]

//        [HttpGet("topic/{topicId}")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByTopic(int topicId)
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetByTopicAsync(topicId);
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting messages by topic {TopicId}", topicId);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }


//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpGet("published")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetPublishedMessages()
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetPublishedMessagesAsync();
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting published messages");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpGet("pending-approval")]
//        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetPendingApprovalMessages()
//        {
//            try
//            {
//                var messages = await _baseMessageRepository.GetPendingApprovalAsync();
//                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
//                return Ok(messageDtos);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting pending approval messages");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }


//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpPost("create")]
//        public async Task<ActionResult<BaseMessageGetDto>> CreateBaseMessage([FromBody] BaseMessageCreateDto dto)
//        {
//            try
//            {
//                var message = new BaseMessage
//                {
//                    MessageType = dto.MessageType,
//                    Description = dto.Description,
//                    HexFileContent = dto.HexFileContent,
//                    HexFileName = dto.HexFileName,
//                    Version = dto.Version,
//                    TopicId = dto.TopicId,
//                    UploaderId = dto.UploaderId,
//                    Status = dto.Status,
//                    CreatedAt = DateTime.UtcNow
//                };

//                var created = await _baseMessageRepository.AddAsync(message);
//                var messageDto = _mapper.Map<BaseMessageGetDto>(created);
//                return CreatedAtAction(nameof(GetBaseMessageById), new { id = created.Id }, messageDto);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating base message");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }


//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpPut("{id}")]
//        public async Task<ActionResult<BaseMessageGetDto>> UpdateBaseMessage(int id, [FromBody] BaseMessageUpdateDto dto)
//        {
//            try
//            {
//                var message = await _baseMessageRepository.GetByIdAsync(id);
//                if (message == null)
//                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });

//                if (message.Status != BaseMessageStatus.Draft)
//                    return BadRequest(new { message = "Only draft messages can be updated" });

//                if (dto.MessageType==null)
//                    message.MessageType = dto.MessageType;
//                if (dto.Description != null)
//                    message.Description = dto.Description;
//                if (dto.HexFileContent != null)
//                    message.HexFileContent = dto.HexFileContent;
//                if (!string.IsNullOrEmpty(dto.HexFileName))
//                    message.HexFileName = dto.HexFileName;
//                if (!string.IsNullOrEmpty(dto.Version))
//                    message.Version = dto.Version;
//                if (dto.TopicId.HasValue)
//                    message.TopicId = dto.TopicId.Value;

//                message.UpdatedAt = DateTime.UtcNow;

//                var updated = await _baseMessageRepository.UpdateAsync(message);
//                var messageDto = _mapper.Map<BaseMessageGetDto>(updated);
//                return Ok(messageDto);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating base message {Id}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }


//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpPost("{id}/approve")]
//        public async Task<ActionResult<BaseMessageGetDto>> ApproveMessage(int id, [FromBody] BaseMessageApproveDto dto)
//        {
//            try
//            {
//                var message = await _baseMessageRepository.ApproveMessageAsync(id, dto.ApprovedById);
//                if (message == null)
//                    return NotFound(new { message = $"Message with ID {id} not found or not in Pending status" });

//                var messageDto = _mapper.Map<BaseMessageGetDto>(message);
//                return Ok(messageDto);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error approving message {Id}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [Authorize(Roles = "ADMIN")]
//        [HttpPost("{id}/reject")]
//        public async Task<ActionResult<BaseMessageGetDto>> RejectMessage(int id, [FromBody] BaseMessageRejectDto dto)
//        {
//            try
//            {
//                var message = await _baseMessageRepository.RejectMessageAsync(id, dto.RejectionReason);
//                if (message == null)
//                    return NotFound(new { message = $"Message with ID {id} not found or not in Pending status" });

//                var messageDto = _mapper.Map<BaseMessageGetDto>(message);
//                return Ok(messageDto);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error rejecting message {Id}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        // ✅ POST: api/BaseMessages/5/publish - With MQTT Integration
//        [Authorize(Roles = "ADMIN")]
//        [HttpPost("{id}/publish")]
//        public async Task<ActionResult<BaseMessageGetDto>> PublishMessage([FromBody] BaseMessagePublishDto dto, int id)
//        {
//            try
//            {
//                _logger.LogInformation("📤 Attempting to publish message {MessageId}", id);

//                var message = await _baseMessageRepository.PublishMessageAsync(id, dto.PublisherId);

//                if (message == null)
//                {
//                    _logger.LogWarning("❌ Message {MessageId} not found or not in Approved status", id);
//                    return NotFound(new
//                    {
//                        message = $"Message with ID {id} not found or not in Approved status"
//                    });
//                }

//                var messageDto = _mapper.Map<BaseMessageGetDto>(message);

//                _logger.LogInformation("✅ Message {MessageId} published successfully to topic {TopicName}",
//                    id, message.Topic?.Name);

//                return Ok(new
//                {
//                    success = true,
//                    message = messageDto,
//                    mqttStatus = "Message successfully published to MQTT broker as chunks",
//                    topicName = message.Topic?.Name,
//                    publishedAt = message.PublishedAt,
//                    bytesPublished = message.HexFileContent?.Length ?? 0
//                });
//            }
//            catch (FormatException ex)
//            {
//                _logger.LogError(ex, "❌ Invalid hex file content format for message {MessageId}", id);
//                return BadRequest(new
//                {
//                    success = false,
//                    message = "Invalid hex file content format. Cannot convert to Base64."
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error publishing message {MessageId}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while publishing the message to MQTT broker"
//                    });
//            }
//        }

//        // DELETE: api/BaseMessages/5
//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> DeleteBaseMessage(int id)
//        {
//            try
//            {
//                var deleted = await _baseMessageRepository.DeleteAsync(id);
//                if (!deleted)
//                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting base message {Id}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ProblemDetails
//                    {
//                        Title = "Internal Server Error",
//                        Status = StatusCodes.Status500InternalServerError,
//                        Detail = "An unexpected error occurred while processing your request"
//                    });
//            }
//        }

//        [HttpGet("TotalMessages")]
//        [Authorize(Roles = "ADMIN")]
//        [Authorize(Roles = "DEVELOPER")]
//        public async Task<IActionResult> GetTotalMessages()
//        {
//            var totalMessages = await _baseMessageRepository.GetTotalMessagesCountAsync();
//            return Ok(new { TotalMessages = totalMessages });
//        }
//    }
//}



using AutoMapper;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.DataLayer.Enum;
using Fota.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedProjectDTOs.BaseMessages;
using System.Security.Claims;

namespace StaffAffairs.AWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ✅ Global authorization - كل الـ endpoints تحتاج authentication
    public class BaseMessagesController : ControllerBase
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BaseMessagesController> _logger;

        public BaseMessagesController(
            IBaseMessageRepository baseMessageRepository,
            IMapper mapper,
            IDeveloperRepository developerRepository,
            ILogger<BaseMessagesController> logger)
        {
            _baseMessageRepository = baseMessageRepository;
            _mapper = mapper;
            _logger = logger;
            _developerRepository = developerRepository;
        }

        /// <summary>
        /// ✅ Helper: استخراج Developer ID من الـ Claims
        /// </summary>
        private async Task<int> GetCurrentDeveloperIdAsync()
        {
            // ✅ جرب أكثر من Claim Type
            var identityUserId = User.FindFirst("uid")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(identityUserId))
            {
                _logger.LogWarning("⚠️ User is not authenticated - no uid or NameIdentifier claim found");
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            _logger.LogInformation("🔍 Looking for developer with IdentityUserId: {UserId}", identityUserId);

            var developerId = await _developerRepository
                .GetDeveloperIdByIdentityUserIdAsync(identityUserId);

            if (!developerId.HasValue)
            {
                _logger.LogWarning("⚠️ Developer not found for IdentityUserId: {UserId}", identityUserId);
                throw new UnauthorizedAccessException("Developer not found for this user");
            }

            _logger.LogInformation("✅ Found DeveloperId: {DeveloperId}", developerId.Value);
            return developerId.Value;
        }

        /// <summary>
        /// ✅ Helper: استخراج User Identity ID فقط
        /// </summary>
        private string GetCurrentUserIdentityId()
        {
            var identityUserId = User.FindFirst("uid")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(identityUserId))
            {
                _logger.LogWarning("⚠️ User Identity ID not found in claims");
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            return identityUserId;
        }

        /// <summary>
        /// ✅ Helper: التحقق من الـ Roles
        /// </summary>
        private bool IsAdmin()
        {
            var isAdmin = User.IsInRole("ADMIN");
            _logger.LogInformation("👤 User is admin: {IsAdmin}", isAdmin);
            return isAdmin;
        }

        private bool IsDeveloper()
        {
            var isDeveloper = User.IsInRole("DEVELOPER");
            _logger.LogInformation("👤 User is developer: {IsDeveloper}", isDeveloper);
            return isDeveloper;
        }

        // ============================================
        // GET ENDPOINTS
        // ============================================

        /// <summary>
        /// GET: api/BaseMessages
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMIN,DEVELOPER")] // ✅ OR logic - أي واحد منهم
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetAllBaseMessages()
        {
            try
            {
                // ✅ Log user info
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📋 User {UserId} requesting all base messages", userId);

                var messages = await _baseMessageRepository.GetAllAsync();

                if (messages == null || !messages.Any())
                {
                    _logger.LogInformation("ℹ️ No messages found");
                    return NoContent();
                }

                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);
                _logger.LogInformation("✅ Returning {Count} messages", messageDtos.Count);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "🔒 Unauthorized access attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting all base messages");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/5
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<BaseMessageGetDto>> GetBaseMessageById(int id)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📋 User {UserId} requesting message {MessageId}", userId, id);

                var message = await _baseMessageRepository.GetByIdAsync(id);

                if (message == null)
                {
                    _logger.LogWarning("⚠️ Message {MessageId} not found", id);
                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });
                }

                var messageDto = _mapper.Map<BaseMessageGetDto>(message);
                return Ok(messageDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting base message {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/5/file
        /// </summary>
        [HttpGet("{id}/file")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<BaseMessageWithFileDto>> GetBaseMessageWithFile(int id)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📁 User {UserId} requesting message {MessageId} with file", userId, id);

                var message = await _baseMessageRepository.GetByIdAsync(id);

                if (message == null)
                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });

                var messageDto = _mapper.Map<BaseMessageWithFileDto>(message);
                return Ok(messageDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting base message with file {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/search?name=test
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> SearchBaseMessages([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name cannot be empty" });

            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("🔍 User {UserId} searching messages with name: {Name}", userId, name);

                var messages = await _baseMessageRepository.SearchByNameAsync(name);
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error searching base messages");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/status/Draft
        /// </summary>
        [HttpGet("status/{status}")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByStatus(BaseMessageStatus status)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📊 User {UserId} requesting messages with status: {Status}", userId, status);

                var messages = await _baseMessageRepository.GetByStatusAsync(status);
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting messages by status {Status}", status);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/type/Update
        /// </summary>
        [HttpGet("type/{messageType}")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByType(BaseMessageType messageType)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📊 User {UserId} requesting messages with type: {Type}", userId, messageType);

                var messages = await _baseMessageRepository.GetByMessageTypeAsync(messageType);
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting messages by type {MessageType}", messageType);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/uploader/5
        /// </summary>
        [HttpGet("uploader/{uploaderId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByUploader(int uploaderId)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("👤 Admin {UserId} requesting messages by uploader: {UploaderId}",
                    userId, uploaderId);

                var messages = await _baseMessageRepository.GetByUploaderAsync(uploaderId);
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting messages by uploader {UploaderId}", uploaderId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/topic/5
        /// </summary>
        [HttpGet("topic/{topicId}")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetBaseMessagesByTopic(int topicId)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📂 User {UserId} requesting messages by topic: {TopicId}",
                    userId, topicId);

                var messages = await _baseMessageRepository.GetByTopicAsync(topicId);
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting messages by topic {TopicId}", topicId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/published
        /// </summary>
        [HttpGet("published")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetPublishedMessages()
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📤 User {UserId} requesting published messages", userId);

                var messages = await _baseMessageRepository.GetPublishedMessagesAsync();
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting published messages");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/pending-approval
        /// </summary>
        [HttpGet("pending-approval")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<BaseMessageListDto>>> GetPendingApprovalMessages()
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("⏳ User {UserId} requesting pending approval messages", userId);

                var messages = await _baseMessageRepository.GetPendingApprovalAsync();
                var messageDtos = _mapper.Map<List<BaseMessageListDto>>(messages);

                return Ok(messageDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting pending approval messages");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// GET: api/BaseMessages/TotalMessages
        /// </summary>
        [HttpGet("TotalMessages")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<IActionResult> GetTotalMessages()
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("📊 User {UserId} requesting total messages count", userId);

                var totalMessages = await _baseMessageRepository.GetTotalMessagesCountAsync();

                return Ok(new
                {
                    success = true,
                    totalMessages = totalMessages,
                    requestedBy = userId
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting total messages count");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        // ============================================
        // POST ENDPOINTS
        // ============================================

        /// <summary>
        /// POST: api/BaseMessages/create
        /// ✅ يأخذ UploaderId من الـ Claims تلقائياً
        /// </summary>
        [HttpPost("create")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<BaseMessageGetDto>> CreateBaseMessage([FromBody] BaseMessageCreateDto dto)
        {
            try
            {
                // ✅ استخراج Developer ID من Claims
                var developerId = await GetCurrentDeveloperIdAsync();

                _logger.LogInformation("➕ Developer {DeveloperId} creating new message", developerId);

                var message = new BaseMessage
                {
                    MessageType = dto.MessageType,
                    Description = dto.Description,
                    HexFileContent = dto.HexFileContent,
                    HexFileName = dto.HexFileName,
                    Version = dto.Version,
                    TopicId = dto.TopicId,
                    UploaderId = developerId, // ✅ من الـ Claims
                    Status = dto.Status,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _baseMessageRepository.AddAsync(message);
                var messageDto = _mapper.Map<BaseMessageGetDto>(created);

                _logger.LogInformation("✅ Message {MessageId} created successfully by developer {DeveloperId}",
                    created.Id, developerId);

                return CreatedAtAction(
                    nameof(GetBaseMessageById),
                    new { id = created.Id },
                    messageDto
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "🔒 Unauthorized create attempt");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error creating base message");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred while creating the message" });
            }
        }

        /// <summary>
        /// PUT: api/BaseMessages/5
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<BaseMessageGetDto>> UpdateBaseMessage(int id, [FromBody] BaseMessageUpdateDto dto)
        {
            try
            {
                var developerId = await GetCurrentDeveloperIdAsync();
                _logger.LogInformation("✏️ Developer {DeveloperId} updating message {MessageId}",
                    developerId, id);

                var message = await _baseMessageRepository.GetByIdAsync(id);

                if (message == null)
                {
                    _logger.LogWarning("⚠️ Message {MessageId} not found", id);
                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });
                }

                // ✅ التحقق من الصلاحيات - فقط صاحب الـ message أو Admin
                if (!IsAdmin() && message.UploaderId != developerId)
                {
                    _logger.LogWarning("🔒 Developer {DeveloperId} tried to update message {MessageId} owned by {OwnerId}",
                        developerId, id, message.UploaderId);
                    return Forbid(); // 403 Forbidden
                }

                if (message.Status != BaseMessageStatus.Draft)
                {
                    _logger.LogWarning("⚠️ Attempt to update non-draft message {MessageId}", id);
                    return BadRequest(new { message = "Only draft messages can be updated" });
                }

                // Update fields
                if (dto.MessageType != null)
                    message.MessageType = dto.MessageType;
                if (dto.Description != null)
                    message.Description = dto.Description;
                if (dto.HexFileContent != null)
                    message.HexFileContent = dto.HexFileContent;
                if (!string.IsNullOrEmpty(dto.HexFileName))
                    message.HexFileName = dto.HexFileName;
                if (!string.IsNullOrEmpty(dto.Version))
                    message.Version = dto.Version;
                if (dto.TopicId.HasValue)
                    message.TopicId = dto.TopicId.Value;

                message.UpdatedAt = DateTime.UtcNow;

                var updated = await _baseMessageRepository.UpdateAsync(message);
                var messageDto = _mapper.Map<BaseMessageGetDto>(updated);

                _logger.LogInformation("✅ Message {MessageId} updated successfully", id);

                return Ok(messageDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error updating base message {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// POST: api/BaseMessages/5/approve
        /// ✅ يأخذ ApprovedById من الـ Claims
        /// </summary>
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<BaseMessageGetDto>> ApproveMessage(int id)
        {
            try
            {
                // ✅ استخراج Developer ID من Claims
                var developerId = await GetCurrentDeveloperIdAsync();

                _logger.LogInformation("✅ Developer {DeveloperId} approving message {MessageId}",
                    developerId, id);

                var message = await _baseMessageRepository.ApproveMessageAsync(id, developerId);

                if (message == null)
                {
                    _logger.LogWarning("⚠️ Message {MessageId} not found or not in Pending status", id);
                    return NotFound(new
                    {
                        message = $"Message with ID {id} not found or not in Pending status"
                    });
                }

                var messageDto = _mapper.Map<BaseMessageGetDto>(message);

                _logger.LogInformation("✅ Message {MessageId} approved by developer {DeveloperId}",
                    id, developerId);

                return Ok(new
                {
                    success = true,
                    message = messageDto,
                    approvedBy = developerId,
                    approvedAt = message.ApprovedAt
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error approving message {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// POST: api/BaseMessages/5/reject
        /// ✅ ADMIN only
        /// </summary>
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseMessageGetDto>> RejectMessage(int id, [FromBody] BaseMessageRejectDto dto)
        {
            try
            {
                var userId = GetCurrentUserIdentityId();
                _logger.LogInformation("❌ Admin {UserId} rejecting message {MessageId}", userId, id);

                var message = await _baseMessageRepository.RejectMessageAsync(id, dto.RejectionReason);

                if (message == null)
                {
                    _logger.LogWarning("⚠️ Message {MessageId} not found or not in Pending status", id);
                    return NotFound(new
                    {
                        message = $"Message with ID {id} not found or not in Pending status"
                    });
                }

                var messageDto = _mapper.Map<BaseMessageGetDto>(message);

                _logger.LogInformation("✅ Message {MessageId} rejected by admin {UserId}", id, userId);

                return Ok(new
                {
                    success = true,
                    message = messageDto,
                    rejectedBy = userId,
                    rejectionReason = dto.RejectionReason
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error rejecting message {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// POST: api/BaseMessages/5/publish
        /// ✅ يأخذ PublisherId من الـ Claims
        /// </summary>
        [HttpPost("{id}/publish")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<BaseMessageGetDto>> PublishMessage(int id)
        {
            try
            {
                // ✅ استخراج Developer ID من Claims (Admin في هذه الحالة)
                var developerId = await GetCurrentDeveloperIdAsync();

                _logger.LogInformation("📤 Admin {DeveloperId} attempting to publish message {MessageId}",
                    developerId, id);

                var message = await _baseMessageRepository.PublishMessageAsync(id, developerId);

                if (message == null)
                {
                    _logger.LogWarning("❌ Message {MessageId} not found or not in Approved status", id);
                    return NotFound(new
                    {
                        success = false,
                        message = $"Message with ID {id} not found or not in Approved status"
                    });
                }

                var messageDto = _mapper.Map<BaseMessageGetDto>(message);

                _logger.LogInformation("✅ Message {MessageId} published successfully to topic {TopicName} by {PublisherId}",
                    id, message.Topic?.Name, developerId);

                return Ok(new
                {
                    success = true,
                    message = messageDto,
                    mqttStatus = "Message successfully published to MQTT broker as chunks",
                    topicName = message.Topic?.Name,
                    publishedAt = message.PublishedAt,
                    publishedBy = developerId,
                    bytesPublished = message.HexFileContent?.Length ?? 0
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "❌ Invalid hex file content format for message {MessageId}", id);
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid hex file content format. Cannot convert to Base64."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error publishing message {MessageId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "An unexpected error occurred while publishing the message"
                    });
            }
        }

        // ============================================
        // DELETE ENDPOINT
        // ============================================

        /// <summary>
        /// DELETE: api/BaseMessages/5
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult> DeleteBaseMessage(int id)
        {
            try
            {
                var developerId = await GetCurrentDeveloperIdAsync();
                _logger.LogInformation("🗑️ Developer {DeveloperId} attempting to delete message {MessageId}",
                    developerId, id);

                var message = await _baseMessageRepository.GetByIdAsync(id);

                if (message == null)
                {
                    _logger.LogWarning("⚠️ Message {MessageId} not found", id);
                    return NotFound(new { message = $"BaseMessage with ID {id} not found" });
                }

                // ✅ التحقق من الصلاحيات - فقط صاحب الـ message أو Admin
                if (!IsAdmin() && message.UploaderId != developerId)
                {
                    _logger.LogWarning("🔒 Developer {DeveloperId} tried to delete message {MessageId} owned by {OwnerId}",
                        developerId, id, message.UploaderId);
                    return Forbid();
                }

                var deleted = await _baseMessageRepository.DeleteAsync(id);

                if (!deleted)
                {
                    _logger.LogWarning("⚠️ Failed to delete message {MessageId}", id);
                    return NotFound(new { message = $"Failed to delete message with ID {id}" });
                }

                _logger.LogInformation("✅ Message {MessageId} deleted successfully by {DeveloperId}",
                    id, developerId);

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error deleting base message {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }
    }
}