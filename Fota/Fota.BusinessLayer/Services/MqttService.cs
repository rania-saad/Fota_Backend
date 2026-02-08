using MQTTnet;
using MQTTnet.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet.Protocol;

namespace Fota.Services
{
    public class MqttService
    {
        private readonly IMqttClient _client;
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttService> _logger;

        public IMqttClient Client => _client;

        public MqttService(IConfiguration config, ILogger<MqttService> logger)
        {
            _logger = logger;
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            var host = config["MQTT:Host"] ?? "broker.emqx.io";
            var port = int.Parse(config["MQTT:Port"] ?? "1883");
            var username = config["MQTT:Username"];
            var password = config["MQTT:Password"];
            var useTls = config.GetValue<bool>("MQTT:UseTls", false);

            var optBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer(host, port)
                .WithClientId("FotaApiClient_" + Guid.NewGuid().ToString().Substring(0, 8))
                .WithCleanSession();

            if (!string.IsNullOrEmpty(username))
                optBuilder = optBuilder.WithCredentials(username, password);

            if (useTls)
            {
                optBuilder = optBuilder.WithTlsOptions(o =>
                {
                    o.UseTls();
                });
            }

            _options = optBuilder.Build();

            // Event handlers for connection management
            _client.DisconnectedAsync += async e =>
            {
                _logger.LogWarning("MQTT disconnected. Reconnecting in 5s...");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await _client.ConnectAsync(_options);
                    _logger.LogInformation("MQTT reconnected successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "MQTT reconnect failed");
                }
            };

            _client.ConnectedAsync += e =>
            {
                _logger.LogInformation("MQTT connected successfully");
                return Task.CompletedTask;
            };
        }

        public async Task ConnectAsync()
        {
            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(_options);
                _logger.LogInformation("Connected to MQTT broker.");
            }
        }

        public async Task DisconnectAsync()
        {
            if (_client.IsConnected)
            {
                await _client.DisconnectAsync();
                _logger.LogInformation("Disconnected from MQTT broker");
            }
        }

        /// <summary>
        /// Publishes hex file content as Base64 chunks via MQTT
        /// This is used when SENDING messages TO devices
        /// Default chunk size is 256 bytes (you can adjust as needed)
        /// </summary>
        /// <param name="topic">MQTT topic name</param>
        /// <param name="payloadBase64">Base64 encoded hex file content</param>
        /// <param name="chunkSize">Size of each chunk in bytes (default 256)</param>
        public async Task PublishAsync(string topic, string payloadBase64, int chunkSize = 64)
        {
            if (!_client.IsConnected)
            {
                _logger.LogWarning("MQTT client not connected. Trying to reconnect...");
                await _client.ConnectAsync(_options, CancellationToken.None);
            }

            // Convert Base64 string to bytes
            var bytes = Convert.FromBase64String(payloadBase64);
            int totalChunks = (int)Math.Ceiling((double)bytes.Length / chunkSize);

            _logger.LogInformation("Starting to publish {TotalBytes} bytes in {TotalChunks} chunks to topic {Topic}",
                bytes.Length, totalChunks, topic);

            // Send each chunk
            for (int i = 0; i < totalChunks; i++)
            {
                // Extract chunk bytes
                var chunkBytes = bytes.Skip(i * chunkSize).Take(chunkSize).ToArray();
                var chunkBase64 = Convert.ToBase64String(chunkBytes);

                // Build MQTT message
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(chunkBase64)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithRetainFlag(false)
                    .Build();

                // Publish chunk
                await _client.PublishAsync(msg, CancellationToken.None);

                _logger.LogDebug("Published chunk {ChunkNumber}/{TotalChunks} ({ChunkSize} bytes) to topic {Topic}",
                    i + 1, totalChunks, chunkBytes.Length, topic);

                // Small delay between chunks to avoid overwhelming the broker
                if (i < totalChunks - 1)
                    await Task.Delay(10);
            }

            // Send "finish" message to signal end of transmission
            var finishMsg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload("finish")
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(false)
                .Build();

            await _client.PublishAsync(finishMsg, CancellationToken.None);

            _logger.LogInformation("Successfully published all {TotalChunks} chunks + finish message to topic {Topic}",
                totalChunks, topic);
        }

        /// <summary>
        /// Publishes a simple text message to MQTT (not chunked)
        /// </summary>
        public async Task PublishMessageAsync(string topic, string payload)
        {
            if (!_client.IsConnected)
            {
                _logger.LogWarning("MQTT client not connected. Attempting to connect...");
                await ConnectAsync();
            }

            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(false)
                .Build();

            await _client.PublishAsync(msg, CancellationToken.None);
            _logger.LogInformation("Published message to topic {Topic}", topic);
        }

        /// <summary>
        /// Subscribes to an MQTT topic
        /// </summary>
        public async Task SubscribeAsync(string topic)
        {
            if (!_client.IsConnected)
            {
                await ConnectAsync();
            }

            await _client.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build());

            _logger.LogInformation("Subscribed to topic {Topic}", topic);
        }

        /// <summary>
        /// Unsubscribes from an MQTT topic
        /// </summary>
        public async Task UnsubscribeAsync(string topic)
        {
            if (_client.IsConnected)
            {
                await _client.UnsubscribeAsync(topic);
                _logger.LogInformation("Unsubscribed from topic {Topic}", topic);
            }
        }

        public bool IsConnected => _client.IsConnected;
    }
}