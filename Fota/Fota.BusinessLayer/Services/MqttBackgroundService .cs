using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Fota.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using System.Collections.Concurrent;
using MQTTnet.Client;
using Fota.DataLayer.Enum;
namespace Fota.Services
{
    /// <summary>
    /// Background service that:
    /// 1. Maintains persistent MQTT connection
    /// 2. Subscribes to all topics in the database
    /// 3. RECEIVES chunked messages from devices
    /// 4. Assembles chunks into complete BaseMessage and saves to database
    /// </summary>
    public class MqttBackgroundService : BackgroundService
    {
        private readonly MqttService _mqtt;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MqttBackgroundService> _logger;

        // Dictionary to store incoming chunks per topic
        // Key: TopicName, Value: List of Base64 chunks
        private readonly ConcurrentDictionary<string, List<string>> _messageChunks;

        public MqttBackgroundService(
            MqttService mqtt,
            IServiceScopeFactory scopeFactory,
            ILogger<MqttBackgroundService> logger)
        {
            _mqtt = mqtt;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _messageChunks = new ConcurrentDictionary<string, List<string>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Connect to MQTT broker
                await _mqtt.ConnectAsync();
                _logger.LogInformation("MQTT Background Service started");

                // Set up message received handler BEFORE subscribing
                _mqtt.Client.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;

                // Subscribe to all topics from database
                await SubscribeToAllTopicsAsync();

                // Keep service running
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MQTT Background Service");
            }
            finally
            {
                await _mqtt.DisconnectAsync();
            }
        }

        /// <summary>
        /// Subscribes to all topics from the database
        /// </summary>
        private async Task SubscribeToAllTopicsAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

                var topics = await db.Topics
                    .Where(t => !t.IsDeleted)
                    .Select(t => t.Name)
                    .ToListAsync();

                foreach (var topicName in topics)
                {
                    await _mqtt.SubscribeAsync(topicName);
                    _logger.LogInformation("Subscribed to topic: {TopicName}", topicName);
                }

                _logger.LogInformation("Successfully subscribed to {Count} topics", topics.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to topics");
            }
        }

        /// <summary>
        /// Handles incoming MQTT messages
        /// This method:
        /// 1. Receives chunks from devices
        /// 2. Assembles them into complete message
        /// 3. Saves to database when "finish" is received
        /// </summary>
        private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var topicName = e.ApplicationMessage.Topic;
                var payloadBytes = e.ApplicationMessage.PayloadSegment.ToArray();
                var payloadString = Encoding.UTF8.GetString(payloadBytes);

                _logger.LogDebug("Received message from topic {Topic}: {Payload}",
                    topicName,
                    payloadString.Length > 50 ? payloadString.Substring(0, 50) + "..." : payloadString);

                // Check if this is the "finish" message
                if (payloadString.Trim().Equals("finish", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Received 'finish' signal for topic {Topic}. Assembling message...", topicName);
                    await AssembleAndSaveMessageAsync(topicName);
                    return;
                }

                // Otherwise, it's a chunk - store it
                _messageChunks.AddOrUpdate(
                    topicName,
                    new List<string> { payloadString }, // If topic doesn't exist, create new list
                    (key, existingList) =>
                    {
                        existingList.Add(payloadString); // If topic exists, add to existing list
                        return existingList;
                    });

                _logger.LogDebug("Stored chunk for topic {Topic}. Total chunks: {Count}",
                    topicName, _messageChunks[topicName].Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing incoming MQTT message");
            }
        }

        /// <summary>
        /// Assembles all chunks for a topic into a complete BaseMessage and saves to database
        /// </summary>
        //private async Task AssembleAndSaveMessageAsync(string topicName)
        //{
        //    try
        //    {
        //        // Get all chunks for this topic
        //        if (!_messageChunks.TryRemove(topicName, out var chunks) || chunks == null || chunks.Count == 0)
        //        {
        //            _logger.LogWarning("No chunks found for topic {Topic}", topicName);
        //            return;
        //        }

        //        _logger.LogInformation("Assembling {ChunkCount} chunks for topic {Topic}", chunks.Count, topicName);

        //        // Combine all Base64 chunks into one complete byte array
        //        var allBytes = new List<byte>();
        //        foreach (var chunk in chunks)
        //        {
        //            try
        //            {
        //                var chunkBytes = Convert.FromBase64String(chunk);
        //                allBytes.AddRange(chunkBytes);
        //            }
        //            catch (FormatException ex)
        //            {
        //                _logger.LogError(ex, "Invalid Base64 chunk received for topic {Topic}", topicName);
        //            }
        //        }

        //        var completeHexContent = allBytes.ToArray();

        //        _logger.LogInformation("Assembled complete message: {TotalBytes} bytes for topic {Topic}",
        //            completeHexContent.Length, topicName);

        //        // Save to database
        //        using var scope = _scopeFactory.CreateScope();
        //        var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

        //        // Find the topic
        //        var topic = await db.Topics.FirstOrDefaultAsync(t => t.Name == topicName);
        //        if (topic == null)
        //        {
        //            _logger.LogWarning("Topic {TopicName} not found in database. Skipping message save.", topicName);
        //            return;
        //        }

        //        // Create new BaseMessage with assembled content
        //        var baseMessage = new BaseMessage
        //        {
        //            MessageType = "Standard", // You can determine this from message content if needed
        //            Description = $"Message received from topic {topicName}",
        //            HexFileContent = completeHexContent, // ✅ Save the assembled chunks here
        //            HexFileName = $"received_{topicName}_{DateTime.UtcNow:yyyyMMddHHmmss}.hex",
        //            Version = "1.0", // Extract from message if available
        //            Status = "Received", // Custom status for incoming messages
        //            TopicId = topic.Id,
        //            UploaderId = 1, // You may want to configure a system user ID for incoming messages
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow,
        //            IsDeleted = false
        //        };

        //        db.BaseMessages.Add(baseMessage);
        //        await db.SaveChangesAsync();

        //        _logger.LogInformation(
        //            "✅ Successfully saved BaseMessage (ID: {MessageId}) with {ByteCount} bytes from topic {Topic}",
        //            baseMessage.Id, completeHexContent.Length, topicName);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error assembling and saving message for topic {Topic}", topicName);
        //    }
        //}
        private async Task AssembleAndSaveMessageAsync(string topicName)
        {
            try
            {
                // Get all chunks for this topic
                if (!_messageChunks.TryRemove(topicName, out var chunks) || chunks == null || chunks.Count == 0)
                {
                    _logger.LogWarning("No chunks found for topic {Topic}", topicName);
                    return;
                }

                _logger.LogInformation("Assembling {ChunkCount} chunks for topic {Topic}", chunks.Count, topicName);

                // ✅ دمج كل الـ Base64 chunks في string واحد
                var completeBase64Content = string.Join("", chunks);

                _logger.LogInformation("Assembled complete message: {Base64Length} Base64 chars for topic {Topic}",
                    completeBase64Content.Length, topicName);

                // Save to database
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

                // Find the topic
                var topic = await db.Topics.FirstOrDefaultAsync(t => t.Name == topicName);
                if (topic == null)
                {
                    _logger.LogWarning("Topic {TopicName} not found in database. Skipping message save.", topicName);
                    return;
                }

                // ✅ Create new BaseMessage with assembled Base64 string
                var baseMessage = new BaseMessage
                {
                    MessageType = BaseMessageType.Standard,
                    Description = $"Message received from topic {topicName}",
                    HexFileContent = completeBase64Content, // ✅ احفظ الـ Base64 string مباشرة
                    HexFileName = $"received_{topicName}_{DateTime.UtcNow:yyyyMMddHHmmss}.hex",
                    Version = "1.0",
                    Status = BaseMessageStatus.Published,
                    TopicId = topic.Id,
                    UploaderId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                db.BaseMessages.Add(baseMessage);
                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "✅ Successfully saved BaseMessage (ID: {MessageId}) with {CharCount} Base64 chars from topic {Topic}",
                    baseMessage.Id, completeBase64Content.Length, topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assembling and saving message for topic {Topic}", topicName);
            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MQTT Background Service stopping...");

            // Clean up any remaining chunks
            if (_messageChunks.Count > 0)
            {
                _logger.LogWarning("Service stopping with {Count} incomplete messages. Chunks will be discarded.",
                    _messageChunks.Count);
                _messageChunks.Clear();
            }

            await _mqtt.DisconnectAsync();
            await base.StopAsync(cancellationToken);
            _logger.LogInformation("MQTT Background Service stopped");
        }
    }
}