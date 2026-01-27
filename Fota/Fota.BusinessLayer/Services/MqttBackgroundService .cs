//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Fota.DataLayer.DBContext;
//using Fota.DataLayer.Models;
//using Fota.Models;
//using System.Text;
//using Microsoft.EntityFrameworkCore;
//using MQTTnet;
//using System.Collections.Concurrent;
//using MQTTnet.Client;
//using Fota.DataLayer.Enum;
//namespace Fota.Services
//{
//    /// <summary>
//    /// Background service that:
//    /// 1. Maintains persistent MQTT connection
//    /// 2. Subscribes to all topics in the database
//    /// 3. RECEIVES chunked messages from devices
//    /// 4. Assembles chunks into complete BaseMessage and saves to database
//    /// </summary>
//    public class MqttBackgroundService : BackgroundService
//    {
//        private readonly MqttService _mqtt;
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly ILogger<MqttBackgroundService> _logger;

//        // Dictionary to store incoming chunks per topic
//        // Key: TopicName, Value: List of Base64 chunks
//        private readonly ConcurrentDictionary<string, List<string>> _messageChunks;

//        public MqttBackgroundService(
//            MqttService mqtt,
//            IServiceScopeFactory scopeFactory,
//            ILogger<MqttBackgroundService> logger)
//        {
//            _mqtt = mqtt;
//            _scopeFactory = scopeFactory;
//            _logger = logger;
//            _messageChunks = new ConcurrentDictionary<string, List<string>>();
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            try
//            {
//                // Connect to MQTT broker
//                await _mqtt.ConnectAsync();
//                _logger.LogInformation("MQTT Background Service started");

//                // Set up message received handler BEFORE subscribing
//                _mqtt.Client.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;

//                // Subscribe to all topics from database
//                await SubscribeToAllTopicsAsync();

//                // Keep service running
//                while (!stoppingToken.IsCancellationRequested)
//                {
//                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in MQTT Background Service");
//            }
//            finally
//            {
//                await _mqtt.DisconnectAsync();
//            }
//        }

//        /// <summary>
//        /// Subscribes to all topics from the database
//        /// </summary>
//        private async Task SubscribeToAllTopicsAsync()
//        {
//            try
//            {
//                using var scope = _scopeFactory.CreateScope();
//                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

//                var topics = await db.Topics
//                    .Where(t => !t.IsDeleted)
//                    .Select(t => t.Name)
//                    .ToListAsync();

//                foreach (var topicName in topics)
//                {
//                    await _mqtt.SubscribeAsync(topicName);
//                    _logger.LogInformation("Subscribed to topic: {TopicName}", topicName);
//                }

//                _logger.LogInformation("Successfully subscribed to {Count} topics", topics.Count);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error subscribing to topics");
//            }
//        }

//        /// <summary>
//        /// Handles incoming MQTT messages
//        /// This method:
//        /// 1. Receives chunks from devices
//        /// 2. Assembles them into complete message
//        /// 3. Saves to database when "finish" is received
//        /// </summary>
//        private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
//        {
//            try
//            {
//                var topicName = e.ApplicationMessage.Topic;
//                var payloadBytes = e.ApplicationMessage.PayloadSegment.ToArray();
//                var payloadString = Encoding.UTF8.GetString(payloadBytes);

//                _logger.LogDebug("Received message from topic {Topic}: {Payload}",
//                    topicName,
//                    payloadString.Length > 50 ? payloadString.Substring(0, 50) + "..." : payloadString);

//                // Check if this is the "finish" message
//                if (payloadString.Trim().Equals("finish", StringComparison.OrdinalIgnoreCase))
//                {
//                    _logger.LogInformation("Received 'finish' signal for topic {Topic}. Assembling message...", topicName);
//                    await AssembleAndSaveMessageAsync(topicName);
//                    return;
//                }

//                // Otherwise, it's a chunk - store it
//                _messageChunks.AddOrUpdate(
//                    topicName,
//                    new List<string> { payloadString }, // If topic doesn't exist, create new list
//                    (key, existingList) =>
//                    {
//                        existingList.Add(payloadString); // If topic exists, add to existing list
//                        return existingList;
//                    });

//                _logger.LogDebug("Stored chunk for topic {Topic}. Total chunks: {Count}",
//                    topicName, _messageChunks[topicName].Count);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error processing incoming MQTT message");
//            }
//        }


//        private async Task AssembleAndSaveMessageAsync(string topicName)
//        {
//            try
//            {
//                // Get all chunks for this topic
//                if (!_messageChunks.TryRemove(topicName, out var chunks) || chunks == null || chunks.Count == 0)
//                {
//                    _logger.LogWarning("No chunks found for topic {Topic}", topicName);
//                    return;
//                }

//                _logger.LogInformation("Assembling {ChunkCount} chunks for topic {Topic}", chunks.Count, topicName);

//                // ✅ دمج كل الـ Base64 chunks في string واحد
//                var completeBase64Content = string.Join("", chunks);

//                _logger.LogInformation("Assembled complete message: {Base64Length} Base64 chars for topic {Topic}",
//                    completeBase64Content.Length, topicName);

//                // Save to database
//                using var scope = _scopeFactory.CreateScope();
//                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

//                // Find the topic
//                var topic = await db.Topics.FirstOrDefaultAsync(t => t.Name == topicName);
//                if (topic == null)
//                {
//                    _logger.LogWarning("Topic {TopicName} not found in database. Skipping message save.", topicName);
//                    return;
//                }

//                // ✅ Create new BaseMessage with assembled Base64 string
//                var baseMessage = new BaseMessage
//                {
//                    MessageType = BaseMessageType.Standard,
//                    Description = $"Message received from topic {topicName}",
//                    HexFileContent = completeBase64Content, // ✅ احفظ الـ Base64 string مباشرة
//                    HexFileName = $"received_{topicName}_{DateTime.UtcNow:yyyyMMddHHmmss}.hex",
//                    Version = "1.0",
//                    Status = BaseMessageStatus.Published,
//                    TopicId = topic.Id,
//                    UploaderId = 1,
//                    CreatedAt = DateTime.UtcNow,
//                    UpdatedAt = DateTime.UtcNow,
//                    IsDeleted = false
//                };

//                db.BaseMessages.Add(baseMessage);
//                await db.SaveChangesAsync();

//                _logger.LogInformation(
//                    "✅ Successfully saved BaseMessage (ID: {MessageId}) with {CharCount} Base64 chars from topic {Topic}",
//                    baseMessage.Id, completeBase64Content.Length, topicName);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error assembling and saving message for topic {Topic}", topicName);
//            }
//        }
//        public override async Task StopAsync(CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("MQTT Background Service stopping...");

//            // Clean up any remaining chunks
//            if (_messageChunks.Count > 0)
//            {
//                _logger.LogWarning("Service stopping with {Count} incomplete messages. Chunks will be discarded.",
//                    _messageChunks.Count);
//                _messageChunks.Clear();
//            }

//            await _mqtt.DisconnectAsync();
//            await base.StopAsync(cancellationToken);
//            _logger.LogInformation("MQTT Background Service stopped");
//        }
//    }
//}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Fota.Models;
using System.Text;
using System.Text.Json;
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
    /// 5. RECEIVES diagnostic JSON messages and creates diagnostics
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

                _logger.LogInformation("✅ MQTT service ready to receive messages");

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
        /// Routes messages to appropriate handlers based on payload type
        /// </summary>
        private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var topicName = e.ApplicationMessage.Topic;
                var payloadBytes = e.ApplicationMessage.PayloadSegment.ToArray();
                var payloadString = Encoding.UTF8.GetString(payloadBytes);

                _logger.LogInformation("📥 Received message on topic: {Topic}, Length: {Length}",
                    topicName, payloadString.Length);
                _logger.LogDebug("Payload preview: {Preview}",
                    payloadString.Length > 100 ? payloadString.Substring(0, 100) + "..." : payloadString);

                // ✅ تحقق: هل ده JSON؟
                var trimmedPayload = payloadString.Trim();
                if (trimmedPayload.StartsWith("{") && trimmedPayload.EndsWith("}"))
                {
                    _logger.LogInformation("🔍 Detected JSON payload, attempting to parse...");

                    // محاولة إنشاء Diagnostic
                    var isValidDiagnostic = await TryHandleDiagnosticCreationAsync(payloadString, topicName);
                    if (isValidDiagnostic)
                    {
                        _logger.LogInformation("✅ Successfully processed as diagnostic");
                        return;
                    }

                    // محاولة إنشاء Subscriber
                    var isValidSubscriber = await TryHandleSubscriberCreationAsync(payloadString);
                    if (isValidSubscriber)
                    {
                        _logger.LogInformation("✅ Successfully processed as subscriber");
                        return;
                    }

                    _logger.LogWarning("⚠️ JSON detected but not a valid diagnostic or subscriber format");
                }

                // 🧩 لو مش JSON أو فشل الـ creation، تعامل معاه كـ chunk
                await HandleChunkedMessageAsync(topicName, payloadString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error processing MQTT message on topic {Topic}", e.ApplicationMessage.Topic);
            }
        }

        /// <summary>
        /// محاولة إنشاء diagnostic من JSON
        /// Returns true if successful, false otherwise
        /// </summary>
        private async Task<bool> TryHandleDiagnosticCreationAsync(string jsonPayload, string mqttTopicName)
        {
            try
            {
                _logger.LogInformation("🔧 Attempting to deserialize diagnostic JSON...");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                };

                var diagnosticRequest = JsonSerializer.Deserialize<DiagnosticCreateRequest>(
                    jsonPayload, options);

                if (diagnosticRequest == null)
                {
                    _logger.LogWarning("⚠️ Deserialization returned null");
                    return false;
                }

                // ✅ تحقق من البيانات الأساسية
                if (string.IsNullOrWhiteSpace(diagnosticRequest.Title))
                {
                    _logger.LogWarning("⚠️ Diagnostic title is empty");
                    return false;
                }

                _logger.LogInformation("📝 Diagnostic request details: Title='{Title}', Priority={Priority}, SubscriberId={SubId}",
                    diagnosticRequest.Title, diagnosticRequest.Priority, diagnosticRequest.SubscriberId);

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

                // ✅ استخدم الـ topic من الـ MQTT message نفسه، مش من الـ JSON
                var topic = await db.Topics
                    .FirstOrDefaultAsync(t => t.Name == mqttTopicName && !t.IsDeleted);

                if (topic == null)
                {
                    _logger.LogWarning("⚠️ Topic '{Topic}' not found in database", mqttTopicName);
                    return false;
                }

                _logger.LogInformation("✅ Found topic in database: ID={TopicId}, Name='{TopicName}'",
                    topic.Id, topic.Name);

                // ✅ تحقق من وجود الـ Subscriber
                var subscriber = await db.Subscribers
                    .FirstOrDefaultAsync(s => s.Id == diagnosticRequest.SubscriberId && s.IsActive);

                if (subscriber == null)
                {
                    _logger.LogWarning("⚠️ Subscriber {SubscriberId} not found in database",
                        diagnosticRequest.SubscriberId);
                    return false;
                }

                _logger.LogInformation("✅ Found subscriber: ID={SubId}, Name='{SubName}'",
                    subscriber.Id, subscriber.Name);

                // ✅ إنشاء الـ Diagnostic
                var diagnostic = new Diagnostic
                {
                    Title = diagnosticRequest.Title,
                    Description = diagnosticRequest.Description ?? string.Empty,
                    Priority = diagnosticRequest.Priority,
                    SubscriberId = diagnosticRequest.SubscriberId,
                    TopicId = topic.Id,
                    Status = DiagnosticStatus.Open,
                    CarModel = diagnosticRequest.CarModel,
                    CarBrand = diagnosticRequest.CarBrand,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                db.Diagnostics.Add(diagnostic);
                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "✅✅✅ Diagnostic created successfully! " +
                    "ID={DiagnosticId}, Title='{Title}', Topic='{TopicName}', Subscriber={SubId}, Priority={Priority}",
                    diagnostic.Id, diagnostic.Title, topic.Name, subscriber.Id, diagnostic.Priority);

                return true;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogWarning(jsonEx, "⚠️ Invalid JSON format for diagnostic creation");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error creating diagnostic from MQTT message");
                return false;
            }
        }

        /// <summary>
        /// محاولة إنشاء subscriber من JSON
        /// Returns true if successful, false otherwise
        /// </summary>
        private async Task<bool> TryHandleSubscriberCreationAsync(string jsonPayload)
        {
            try
            {
                _logger.LogInformation("👤 Attempting to deserialize subscriber JSON...");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                };

                var subscriberRequest = JsonSerializer.Deserialize<SubscriberCreateRequest>(
                    jsonPayload, options);

                if (subscriberRequest == null)
                {
                    _logger.LogDebug("⚠️ Deserialization as subscriber returned null");
                    return false;
                }

                // ✅ تحقق من البيانات الأساسية للـ Subscriber
                if (string.IsNullOrWhiteSpace(subscriberRequest.Name) ||
                    string.IsNullOrWhiteSpace(subscriberRequest.Email))
                {
                    _logger.LogDebug("⚠️ Subscriber name or email is empty");
                    return false;
                }

                // ✅ تحقق من صحة الـ Email format
                if (!subscriberRequest.Email.Contains("@"))
                {
                    _logger.LogWarning("⚠️ Invalid email format: {Email}", subscriberRequest.Email);
                    return false;
                }

                _logger.LogInformation("👤 Subscriber request details: Name='{Name}', Email='{Email}'",
                    subscriberRequest.Name, subscriberRequest.Email);

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

                // ✅ تحقق من عدم وجود email مكرر
                var existingSubscriber = await db.Subscribers
                    .FirstOrDefaultAsync(s => s.Email == subscriberRequest.Email && s.IsActive);

                if (existingSubscriber != null)
                {
                    _logger.LogWarning("⚠️ Subscriber with email '{Email}' already exists (ID: {Id})",
                        subscriberRequest.Email, existingSubscriber.Id);
                    return false;
                }

                // ✅ إنشاء الـ Subscriber
                var subscriber = new Subscriber
                {
                    Name = subscriberRequest.Name,
                    Email = subscriberRequest.Email,
                    PhoneNumber = subscriberRequest.PhoneNumber,
                    location = subscriberRequest.Location,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                db.Subscribers.Add(subscriber);
                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "✅✅✅ Subscriber created successfully! " +
                    "ID={SubscriberId}, Name='{Name}', Email='{Email}', Location='{Location}'",
                    subscriber.Id, subscriber.Name, subscriber.Email, subscriber.location ?? "N/A");

                return true;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogDebug(jsonEx, "⚠️ Invalid JSON format for subscriber creation");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error creating subscriber from MQTT message");
                return false;
            }
        }

        /// <summary>
        /// Handles chunked messages (existing functionality)
        /// </summary>
        private async Task HandleChunkedMessageAsync(string topicName, string payloadString)
        {
            // Check if this is the "finish" message
            if (payloadString.Trim().Equals("finish", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("🏁 Received 'finish' signal for topic {Topic}. Assembling message...", topicName);
                await AssembleAndSaveMessageAsync(topicName);
                return;
            }

            // Otherwise, it's a chunk - store it
            _messageChunks.AddOrUpdate(
                topicName,
                new List<string> { payloadString },
                (key, existingList) =>
                {
                    existingList.Add(payloadString);
                    return existingList;
                });

            _logger.LogDebug("📦 Stored chunk for topic {Topic}. Total chunks: {Count}",
                topicName, _messageChunks[topicName].Count);
        }

        /// <summary>
        /// Assembles chunked messages and saves to database
        /// </summary>
        private async Task AssembleAndSaveMessageAsync(string topicName)
        {
            try
            {
                // Get all chunks for this topic
                if (!_messageChunks.TryRemove(topicName, out var chunks) || chunks == null || chunks.Count == 0)
                {
                    _logger.LogWarning("⚠️ No chunks found for topic {Topic}", topicName);
                    return;
                }

                _logger.LogInformation("🔧 Assembling {ChunkCount} chunks for topic {Topic}", chunks.Count, topicName);

                // Merge all Base64 chunks into one string
                var completeBase64Content = string.Join("", chunks);

                _logger.LogInformation("✅ Assembled complete message: {Base64Length} Base64 chars for topic {Topic}",
                    completeBase64Content.Length, topicName);

                // Save to database
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FOTADbContext>();

                // Find the topic
                var topic = await db.Topics.FirstOrDefaultAsync(t => t.Name == topicName && !t.IsDeleted);
                if (topic == null)
                {
                    _logger.LogWarning("⚠️ Topic {TopicName} not found in database. Skipping message save.", topicName);
                    return;
                }

                // Create new BaseMessage with assembled Base64 string
                var baseMessage = new BaseMessage
                {
                    MessageType = BaseMessageType.Standard,
                    Description = $"Message received from topic {topicName}",
                    HexFileContent = completeBase64Content,
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
                    "✅✅✅ Successfully saved BaseMessage (ID: {MessageId}) with {CharCount} Base64 chars from topic {Topic}",
                    baseMessage.Id, completeBase64Content.Length, topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error assembling and saving message for topic {Topic}", topicName);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🛑 MQTT Background Service stopping...");

            // Clean up any remaining chunks
            if (_messageChunks.Count > 0)
            {
                _logger.LogWarning("⚠️ Service stopping with {Count} incomplete messages. Chunks will be discarded.",
                    _messageChunks.Count);
                _messageChunks.Clear();
            }

            await _mqtt.DisconnectAsync();
            await base.StopAsync(cancellationToken);
            _logger.LogInformation("✅ MQTT Background Service stopped");
        }
    }

    /// <summary>
    /// DTO for diagnostic creation via MQTT
    /// </summary>
    public class DiagnosticCreateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DiagnosticPriority Priority { get; set; }
        public int SubscriberId { get; set; }
        public string? TopicName { get; set; } // ✅ Optional - سنستخدم topic من MQTT
        public string? CarModel { get; set; }
        public string? CarBrand { get; set; }
    }

    /// <summary>
    /// DTO for subscriber creation via MQTT
    /// </summary>
    public class SubscriberCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
    }
}