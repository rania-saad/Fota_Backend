

//using MQTTnet;
//using MQTTnet.Client;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MQTTnet.Protocol;

//namespace Fota.Services
//{
//    public class MqttService
//    {
//        private readonly IMqttClient _client;
//        private readonly MqttClientOptions _options;
//        private readonly ILogger<MqttService> _logger;

//        public IMqttClient Client => _client;

//        public MqttService(IConfiguration config, ILogger<MqttService> logger)
//        {
//            _logger = logger;
//            var factory = new MqttFactory();
//            _client = factory.CreateMqttClient();

//            var host = config["MQTT:Host"];
//            var port = int.Parse(config["MQTT:Port"] ?? "1883");
//            var username = config["MQTT:Username"];
//            var password = config["MQTT:Password"];
//            var useTls = config.GetValue<bool>("MQTT:UseTls", false);


//            var optBuilder = new MqttClientOptionsBuilder()
//                .WithTcpServer("broker.emqx.io", 1883) // بدل localhost
//                .WithClientId("myApiClient")
//                .WithCleanSession();




//            if (!string.IsNullOrEmpty(username))
//                optBuilder = optBuilder.WithCredentials(username, password);

//            if (useTls)
//            {
//                optBuilder = optBuilder.WithTlsOptions(o =>
//                {
//                    o.UseTls(); // ✅ الطريقة الصحيحة في v4
//                });
//            }

//            _options = optBuilder.Build();

//            // ✅ event handlers بدل UseDisconnectedHandler
//            _client.DisconnectedAsync += async e =>
//            {
//                _logger.LogWarning("MQTT disconnected. Reconnecting in 5s...");
//                await Task.Delay(TimeSpan.FromSeconds(5));
//                try { await _client.ConnectAsync(_options); }
//                catch (Exception ex) { _logger.LogError(ex, "Reconnect failed"); }
//            };
//        }

//        public async Task ConnectAsync()
//        {
//            if (!_client.IsConnected)
//            {
//                await _client.ConnectAsync(_options);
//                _logger.LogInformation("Connected to MQTT broker.");
//            }
//        }



//        //public async Task PublishAsync(string topic, string payloadBase64, int chunkSize = 64)
//        //{
//        //    if (!_client.IsConnected)
//        //    {
//        //        _logger.LogWarning("MQTT client not connected. Trying to reconnect...");
//        //        await _client.ConnectAsync(_options, CancellationToken.None);
//        //    }

//        //    var bytes = Convert.FromBase64String(payloadBase64);
//        //    int totalChunks = (int)Math.Ceiling((double)bytes.Length / chunkSize);

//        //    for (int i = 0; i < totalChunks; i++)
//        //    {
//        //        var chunkBytes = bytes.Skip(i * chunkSize).Take(chunkSize).ToArray();
//        //        var chunkBase64 = Convert.ToBase64String(chunkBytes);

//        //        var msg = new MqttApplicationMessageBuilder()
//        //            .WithTopic(topic)
//        //            .WithPayload(chunkBase64)
//        //            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
//        //            .Build();

//        //        await _client.PublishAsync(msg, CancellationToken.None);

//        //        _logger.LogInformation("🚀 Published chunk {i}/{totalChunks} to topic {topic}", i + 1, totalChunks, topic);
//        //    }
//        //}
//        public async Task PublishAsync(string topic, string payloadBase64, int chunkSize = 64)
//        {
//            if (!_client.IsConnected)
//            {
//                _logger.LogWarning("MQTT client not connected. Trying to reconnect...");
//                await _client.ConnectAsync(_options, CancellationToken.None);
//            }

//            var bytes = Convert.FromBase64String(payloadBase64);
//            int totalChunks = (int)Math.Ceiling((double)bytes.Length / chunkSize);

//            for (int i = 0; i < totalChunks; i++)
//            {
//                var chunkBytes = bytes.Skip(i * chunkSize).Take(chunkSize).ToArray();
//                var chunkBase64 = Convert.ToBase64String(chunkBytes);

//                var msg = new MqttApplicationMessageBuilder()
//                    .WithTopic(topic)
//                    .WithPayload(chunkBase64)
//                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
//                    .Build();

//                await _client.PublishAsync(msg, CancellationToken.None);

//                _logger.LogInformation(" Published chunk {i}/{totalChunks} to topic {topic}", i + 1, totalChunks, topic);
//            }

//            // ✅ بعد آخر chunk → ابعتي رسالة "finish"
//            var finishMsg = new MqttApplicationMessageBuilder()
//                .WithTopic(topic)
//                .WithPayload("finish")
//                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
//                .Build();

//            await _client.PublishAsync(finishMsg, CancellationToken.None);

//            _logger.LogInformation(" Sent 'finish' message to topic {topic}", topic);
//        }

//        public async Task SubscribeAsync(string topic)
//        {
//            await _client.SubscribeAsync(new MqttTopicFilterBuilder()
//                .WithTopic(topic)
//                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
//                .Build());

//            _logger.LogInformation("Subscribed to topic {topic}", topic);
//        }
//    }
//}

