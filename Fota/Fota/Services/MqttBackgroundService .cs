

//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Fota.Data;
//using Fota.Models;
//using System.Text;
//using Microsoft.EntityFrameworkCore;
//using MQTTnet;

//namespace Fota.Services
//{
//    public class MqttBackgroundService : BackgroundService
//    {
//        private readonly MqttService _mqtt;
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly ILogger<MqttBackgroundService> _logger;

//        public MqttBackgroundService(MqttService mqtt, IServiceScopeFactory scopeFactory, ILogger<MqttBackgroundService> logger)
//        {
//            _mqtt = mqtt;
//            _scopeFactory = scopeFactory;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            await _mqtt.ConnectAsync();

//            //_mqtt.Client.ApplicationMessageReceivedAsync += async e =>
//            //{
//            //    try
//            //    {
//            //        var topicName = e.ApplicationMessage.Topic;
//            //        var payloadBytes = e.ApplicationMessage.Payload ?? Array.Empty<byte>();
//            //        var payloadString = Encoding.UTF8.GetString(payloadBytes);

//            //        using var scope = _scopeFactory.CreateScope();
//            //        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//            //        var topic = await db.Topics.FirstOrDefaultAsync(t => t.Name == topicName);
//            //        if (topic == null)
//            //        {
//            //            topic = new Topic { Name = topicName };
//            //            db.Topics.Add(topic);
//            //            await db.SaveChangesAsync();
//            //        }

//            //        var msg = new Message
//            //        {
//            //            TopicId = topic.TopicId,
//            //            Payload = payloadString,
//            //            CreatedAt = DateTime.UtcNow
//            //        };

//            //        db.Messages.Add(msg);
//            //        await db.SaveChangesAsync();

//            //        _logger.LogInformation("Saved incoming message from topic {topic}", topicName);
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        _logger.LogError(ex, "Error processing incoming MQTT message");
//            //    }
//            //};


//            _mqtt.Client.ApplicationMessageReceivedAsync += async e =>
//            {
//                using var scope = _scopeFactory.CreateScope();
//                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

//                var message = new Message
//                {
//                    Payload = payload,
//                    TopicId = 1, // أو تجيبي الـ TopicId من DB حسب الـ TopicName
//                    PublisherId = 1, // 🔴 خليها Publisher موجود فعلاً في DB
//                    CreatedAt = DateTime.UtcNow
//                };

//                db.Messages.Add(message);
//                await db.SaveChangesAsync();
//            };


//            using (var scope = _scopeFactory.CreateScope())
//            {
//                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//                var topics = await db.Topics.Select(t => t.Name).ToListAsync();
//                foreach (var t in topics)
//                {
//                    await _mqtt.SubscribeAsync(t);
//                }
//            }

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                await Task.Delay(TimeSpan.FromSeconds(.1), stoppingToken);
//            }
//        }
//    }
//}
