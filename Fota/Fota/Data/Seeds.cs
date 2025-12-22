//using Fota.Models;
//using Microsoft.EntityFrameworkCore;

//namespace Fota.Data
//{
//    public static class DbSeeder
//    {
//        public static void Seed(ModelBuilder modelBuilder)
//        {
//            // ✅ Topics
//            modelBuilder.Entity<Topic>().HasData(
//                new Topic { TopicId = 1, Name = "IoT" },
//                new Topic { TopicId = 2, Name = "AI" },
//                new Topic { TopicId = 3, Name = "Cloud Computing" }
//            );

//            // ✅ Publishers
//            modelBuilder.Entity<Publisher>().HasData(
//                new Publisher { PublisherId = 1, Name = "Publisher A", Description = "Publishes IoT data" },
//                new Publisher { PublisherId = 2, Name = "Publisher B", Description = "Publishes AI updates" }
//            );

//            // ✅ Subscribers
//            modelBuilder.Entity<Subscriber>().HasData(
//                new Subscriber { SubscriberId = 1, Name = "Subscriber X", Description = "Interested in IoT" },
//                new Subscriber { SubscriberId = 2, Name = "Subscriber Y", Description = "Interested in AI & Cloud" }
//            );

//            // ✅ Messages
//            modelBuilder.Entity<Message>().HasData(

//                    new Message { MessageId = 1, TopicId = 1, PublisherId = 1, Payload = "Temperature: 25°C", CreatedAt = new DateTime(2025, 01, 01) },

//                    // ✅ صح
//                    new Message { MessageId = 2, TopicId = 1, PublisherId = 1, Payload = "Temperature: 25°C", CreatedAt = new DateTime(2025, 01, 01) }

//                );

//            // ✅ Subscriptions
//            modelBuilder.Entity<Subscription>().HasData(
//                new Subscription { SubscriptionId = 1, SubscriberId = 1, TopicId = 1 }, // Subscriber X → IoT
//                new Subscription { SubscriptionId = 2, SubscriberId = 2, TopicId = 2 }, // Subscriber Y → AI
//                new Subscription { SubscriptionId = 3, SubscriberId = 2, TopicId = 3 }  // Subscriber Y → Cloud
//            );
//        }
//    }
//}
