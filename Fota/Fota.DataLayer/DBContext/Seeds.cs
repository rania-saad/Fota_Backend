////using Fota.DataLayer.Models;
////using Fota.Models;
////using Microsoft.EntityFrameworkCore;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace Fota.DataLayer.DBContext
////{
////    public partial class FOTADbContext
////    {
////        protected override void OnModelCreating(ModelBuilder modelBuilder)
////        {
////            base.OnModelCreating(modelBuilder);
////            //
////            // Loop through all foreign keys in the model
////            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
////            {
////                // Loop through all foreign key relationships
////                foreach (var foreignKey in entityType.GetForeignKeys())
////                {
////                    // Set the delete behavior to Restrict for each FK
////                    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
////                }
////            }

////            // ----------------------
////            modelBuilder.Entity<Admin>().HasData(
////                new Admin { Id = 1, Name = "Admin One", Email = "admin1@example.com" },
////                new Admin { Id = 2, Name = "Admin Two", Email = "admin2@example.com" },
////                new Admin { Id = 3, Name = "Admin Three", Email = "admin3@example.com" },
////                new Admin { Id = 4, Name = "Admin Four", Email = "admin4@example.com" },
////                new Admin { Id = 5, Name = "Admin Five", Email = "admin5@example.com" }
////            );

////            // ----------------------
////            // Developers
////            // ----------------------
////            modelBuilder.Entity<Developer>().HasData(
////                new Developer { Id = 1, Name = "Dev One", Email = "dev1@example.com" },
////                new Developer { Id = 2, Name = "Dev Two", Email = "dev2@example.com" },
////                new Developer { Id = 3, Name = "Dev Three", Email = "dev3@example.com" },
////                new Developer { Id = 4, Name = "Dev Four", Email = "dev4@example.com" },
////                new Developer { Id = 5, Name = "Dev Five", Email = "dev5@example.com" }
////            );

////            // ----------------------
////            // Publishers
////            // ----------------------
////            modelBuilder.Entity<Publisher>().HasData(
////                new Publisher { PublisherId = 1, Name = "Publisher One", Email = "pub1@example.com" },
////                new Publisher { PublisherId = 2, Name = "Publisher Two", Email = "pub2@example.com" },
////                new Publisher { PublisherId = 3, Name = "Publisher Three", Email = "pub3@example.com" },
////                new Publisher { PublisherId = 4, Name = "Publisher Four", Email = "pub4@example.com" },
////                new Publisher { PublisherId = 5, Name = "Publisher Five", Email = "pub5@example.com" }
////            );

////            // ----------------------
////            // Subscribers
////            // ----------------------
////            modelBuilder.Entity<Subscriber>().HasData(
////                new Subscriber { Id = 1, Name = "Subscriber One", Email = "sub1@example.com" },
////                new Subscriber { Id = 2, Name = "Subscriber Two", Email = "sub2@example.com" },
////                new Subscriber { Id = 3, Name = "Subscriber Three", Email = "sub3@example.com" },
////                new Subscriber { Id = 4, Name = "Subscriber Four", Email = "sub4@example.com" },
////                new Subscriber { Id = 5, Name = "Subscriber Five", Email = "sub5@example.com" }
////            );

////            // ----------------------
////            // Projects
////            // ----------------------
////  //          modelBuilder.Entity<Project>().HasData(
////  //                new Project { Id = 1, Name = "Project Alpha", Description = "Alpha project description", CreatedByAdminId = 1 },
////  //                new Project { Id = 2, Name = "Project Beta", Description = "Beta project description", CreatedByAdminId = 2 },
////  //                new Project { Id = 3, Name = "Project Gamma", Description = "Gamma project description", CreatedByAdminId = 3 },
////  //                new Project { Id = 4, Name = "Project Delta", Description = "Delta project description", CreatedByAdminId = 4 },
////  //                new Project { Id = 5, Name = "Project Epsilon", Description = "Epsilon project description", CreatedByAdminId = 5 }
////  //);

////            // ----------------------
////            // Teams
////            // ----------------------
////            modelBuilder.Entity<Team>().HasData(
////                new Team { Id = 1, Name = "Team A" },
////                new Team { Id = 2, Name = "Team B" },
////                new Team { Id = 3, Name = "Team C" },
////                new Team { Id = 4, Name = "Team D" },
////                new Team { Id = 5, Name = "Team E" }
////            );

////            // ----------------------
////            // Topics
////            // ----------------------
////            modelBuilder.Entity<Topic>().HasData(
////                new Topic { Id = 1, Name = "Engine Control", Description = "Engine module updates", IsDeleted = false },
////                new Topic { Id = 2, Name = "Battery System", Description = "Battery diagnostics", IsDeleted = false },
////                new Topic { Id = 3, Name = "Navigation", Description = "GPS and navigation messages",  IsDeleted = false },
////                new Topic { Id = 4, Name = "Security", Description = "Vehicle security updates", IsDeleted = false },
////                new Topic { Id = 5, Name = "Sensors", Description = "On-board sensor calibration",  IsDeleted = false }
////            );

////            // ----------------------
////            // BaseMessages
////            // ----------------------
////            modelBuilder.Entity<BaseMessage>().HasData(
////      new BaseMessage
////      {
////          Id = 1,
////          Description = "Engine Update V1",
////          HexFileName = "engine_v1.hex",
////          HexFileContent = new byte[] { 0b10110101 }, // مثال كمصفوفة بايت
////          Version = "1.0.0",
////          CreatedTime = new DateTime(2025, 11, 21, 16, 30, 0),
////          TopicId = 1,
////          UploaderId = 1,
////          PublisherId = 1,
////          UploadedAt = new DateTime(2025, 11, 21, 16, 30, 0),
////          IsApproved = true,
////          IsDeleted = false
////      },
////      new BaseMessage
////      {
////          Id = 2,
////          Description = "Battery Patch",
////          HexFileName = "battery_patch.hex",
////          HexFileContent = Convert.FromBase64String("MTAxMDAxMTAxMDAxMDEwMTAxMA=="),
////          Version = "1.1.0",
////          CreatedTime = new DateTime(2025, 11, 21, 16, 30, 0),
////          TopicId = 2,
////          UploaderId = 2,
////          PublisherId = 2,
////          UploadedAt = new DateTime(2025, 11, 21, 16, 30, 0),
////          IsApproved = false,
////          IsDeleted = false
////      },
////      new BaseMessage
////      {
////          Id = 3,
////          Description = "GPS Fix",
////          HexFileName = "gps_fix.hex",
////          HexFileContent = Convert.FromBase64String("MTAxMDAxMTAxMDAxMDEwMTAxMA=="),
////          Version = "1.0.5",
////          CreatedTime = new DateTime(2025, 11, 21, 16, 30, 0),
////          TopicId = 3,
////          UploaderId = 3,
////          PublisherId = 3,
////          UploadedAt = new DateTime(2025, 11, 21, 16, 30, 0),
////          IsApproved = true,
////          IsDeleted = false
////      }
////  // وهكذا لباقي الرسائل
////  );


////            // ----------------------
////            // Diagnostics
////            // ----------------------
////            modelBuilder.Entity<Diagnostic>().HasData(
////                new Diagnostic { Id = 1, Description = "Low battery detected", TopicId = 2, IsSolved = false, SubscriberId = 1, AssignedByAdminId = 1 },
////                new Diagnostic { Id = 2, Description = "GPS signal loss", TopicId = 3, IsSolved = true, SubscriberId = 2, AssignedByAdminId = 2 },
////                new Diagnostic { Id = 3, Description = "Engine temperature high", TopicId = 4, IsSolved = false, SubscriberId = 3, AssignedByAdminId = 3 },
////                new Diagnostic { Id = 4, Description = "Door sensor malfunction", TopicId = 5, IsSolved = true, SubscriberId = 4, AssignedByAdminId = 4 },
////                new Diagnostic { Id = 5, Description = "Security breach attempt", TopicId = 1, IsSolved = false, SubscriberId = 5, AssignedByAdminId = 5 }
////            );

////            // ----------------------
////            // MessageDeliveries
////            // ----------------------
////            modelBuilder.Entity<MessageDelivery>().HasData(
////                new MessageDelivery { Id = 1, MessageId = 1, SubscriberId = 1, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = false },
////                new MessageDelivery { Id = 2, MessageId = 2, SubscriberId = 2, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = true },
////                new MessageDelivery { Id = 3, MessageId = 3, SubscriberId = 3, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = false },
////                new MessageDelivery { Id = 4, MessageId = 2, SubscriberId = 4, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = true },
////                new MessageDelivery { Id = 5, MessageId = 1, SubscriberId = 5, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = false }
////            );




////            // فقط الحاجات اللي EF Conventions مش هتعرفها لوحدها

////            // Query Filter للـ Soft Delete
////            modelBuilder.Entity<BaseMessage>()
////                .HasQueryFilter(m => !m.IsDeleted);

////            // Unique Indexes
////            modelBuilder.Entity<Admin>()
////                .HasIndex(a => a.Email)
////                .IsUnique()
////                .HasFilter("[Email] IS NOT NULL");

////            modelBuilder.Entity<Developer>()
////                .HasIndex(d => d.Email)
////                .IsUnique()
////                .HasFilter("[Email] IS NOT NULL");

////            modelBuilder.Entity<Publisher>()
////                .HasIndex(p => p.Email)
////                .IsUnique()
////                .HasFilter("[Email] IS NOT NULL");

////            modelBuilder.Entity<Subscriber>()
////                .HasIndex(s => s.Email)
////                .IsUnique()
////                .HasFilter("[Email] IS NOT NULL");

////            modelBuilder.Entity<Project>()
////                .HasIndex(p => p.Name)
////                .IsUnique();

////            modelBuilder.Entity<Topic>()
////                .HasQueryFilter(t => !t.IsDeleted)
////                .HasIndex(t => new { t.ProjectId, t.Name })
////                .IsUnique();

////            // TPH Inheritance Discriminator
////            modelBuilder.Entity<BaseMessage>()
////                .HasDiscriminator<string>("MessageType")
////                .HasValue<BaseMessage>("BaseMessage")
////                .HasValue<DiagnosticMessage>("DiagnosticMessage");



////            modelBuilder.Entity<Topic>()
////                .HasMany(t => t.Subscribers)
////                .WithMany(s => s.Topics)
////                .UsingEntity(j => j.ToTable("TopicSubscribers"));

////            modelBuilder.Entity<Project>()
////                .HasMany(p => p.Teams)
////                .WithMany(t => t.Projects)
////                .UsingEntity(j => j.ToTable("ProjectTeams"));
////        }
////    }
////}


//using Fota.DataLayer.Models;
//using Fota.Models;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace Fota.DataLayer.DBContext
//{
//    public partial class FOTADbContext
//    {
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // منع الحذف التلقائي لجميع الفورين كي
//            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
//            {
//                foreach (var foreignKey in entityType.GetForeignKeys())
//                {
//                    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
//                }
//            }

//            // ----------------------
//            // Admins
//            // ----------------------
//            modelBuilder.Entity<Admin>().HasData(
//                new Admin { Id = 1, Name = "Admin One", Email = "admin1@example.com" },
//                new Admin { Id = 2, Name = "Admin Two", Email = "admin2@example.com" },
//                new Admin { Id = 3, Name = "Admin Three", Email = "admin3@example.com" },
//                new Admin { Id = 4, Name = "Admin Four", Email = "admin4@example.com" },
//                new Admin { Id = 5, Name = "Admin Five", Email = "admin5@example.com" }
//            );

//            // ----------------------
//            // Developers
//            // ----------------------
//            modelBuilder.Entity<Developer>().HasData(
//                new Developer { Id = 1, Name = "Dev One", Email = "dev1@example.com" },
//                new Developer { Id = 2, Name = "Dev Two", Email = "dev2@example.com" },
//                new Developer { Id = 3, Name = "Dev Three", Email = "dev3@example.com" },
//                new Developer { Id = 4, Name = "Dev Four", Email = "dev4@example.com" },
//                new Developer { Id = 5, Name = "Dev Five", Email = "dev5@example.com" }
//            );

//            // ----------------------
//            // Publishers
//            // ----------------------
//            modelBuilder.Entity<Publisher>().HasData(
//                new Publisher { PublisherId = 1, Name = "Publisher One", Email = "pub1@example.com" },
//                new Publisher { PublisherId = 2, Name = "Publisher Two", Email = "pub2@example.com" },
//                new Publisher { PublisherId = 3, Name = "Publisher Three", Email = "pub3@example.com" },
//                new Publisher { PublisherId = 4, Name = "Publisher Four", Email = "pub4@example.com" },
//                new Publisher { PublisherId = 5, Name = "Publisher Five", Email = "pub5@example.com" }
//            );

//            // ----------------------
//            // Subscribers
//            // ----------------------
//            modelBuilder.Entity<Subscriber>().HasData(
//                new Subscriber { Id = 1, Name = "Subscriber One", Email = "sub1@example.com" },
//                new Subscriber { Id = 2, Name = "Subscriber Two", Email = "sub2@example.com" },
//                new Subscriber { Id = 3, Name = "Subscriber Three", Email = "sub3@example.com" },
//                new Subscriber { Id = 4, Name = "Subscriber Four", Email = "sub4@example.com" },
//                new Subscriber { Id = 5, Name = "Subscriber Five", Email = "sub5@example.com" }
//            );

//            // ----------------------
//            // Topics
//            // ----------------------
//            modelBuilder.Entity<Topic>().HasData(
//                new Topic { Id = 1, Name = "Engine Control", Description = "Engine module updates", IsDeleted = false },
//                new Topic { Id = 2, Name = "Battery System", Description = "Battery diagnostics", IsDeleted = false },
//                new Topic { Id = 3, Name = "Navigation", Description = "GPS and navigation messages", IsDeleted = false },
//                new Topic { Id = 4, Name = "Security", Description = "Vehicle security updates", IsDeleted = false },
//                new Topic { Id = 5, Name = "Sensors", Description = "On-board sensor calibration", IsDeleted = false }
//            );

//            // ----------------------
//            // Teams (مرتبط بكل Topic)
//            // ----------------------
//            modelBuilder.Entity<Team>().HasData(
//                new Team { Id = 1, Name = "Team A", LeadId = 1, /* TopicId = 1 */ },
//                new Team { Id = 2, Name = "Team B", LeadId = 2, /* TopicId = 1 */ },
//                new Team { Id = 3, Name = "Team C", LeadId = 3, /* TopicId = 2 */ },
//                new Team { Id = 4, Name = "Team D", LeadId = 4, /* TopicId = 3 */ },
//                new Team { Id = 5, Name = "Team E", LeadId = 5, /* TopicId = 4 */ }
//            );

//            // ----------------------
//            // Developer ↔ Team Lead (One-to-Many)
//            modelBuilder.Entity<Team>()
//                .HasOne(t => t.Lead)
//                .WithMany() // ما فيش collection في Developer
//                .HasForeignKey(t => t.LeadId)
//                .OnDelete(DeleteBehavior.NoAction);




//            // ----------------------
//            // Team ↔ Topic Many-to-One
//            // ----------------------
//            modelBuilder.Entity<Team>()
//                .HasOne<Topic>()
//                .WithMany(t => t.Teams)
//                .HasForeignKey("TopicId")
//                .OnDelete(DeleteBehavior.NoAction);

//            // ----------------------
//            // BaseMessages
//            // ----------------------
//            modelBuilder.Entity<BaseMessage>().HasData(
//                new BaseMessage
//                {
//                    Id = 1,
//                    Description = "Engine Update V1",
//                    HexFileName = "engine_v1.hex",
//                    HexFileContent = new byte[] { 0b10110101 },
//                    Version = "1.0.0",
//                    CreatedTime = new DateTime(2025, 11, 21, 16, 30, 0),
//                    TopicId = 1,
//                    UploaderId = 1,
//                    PublisherId = 1,
//                    UploadedAt = new DateTime(2025, 11, 21, 16, 30, 0),
//                    IsApproved = true,
//                    IsDeleted = false
//                },
//                new BaseMessage
//                {
//                    Id = 2,
//                    Description = "Battery Patch",
//                    HexFileName = "battery_patch.hex",
//                    HexFileContent = Convert.FromBase64String("MTAxMDAxMTAxMDAxMDEwMTAxMA=="),
//                    Version = "1.1.0",
//                    CreatedTime = new DateTime(2025, 11, 21, 16, 30, 0),
//                    TopicId = 2,
//                    UploaderId = 2,
//                    PublisherId = 2,
//                    UploadedAt = new DateTime(2025, 11, 21, 16, 30, 0),
//                    IsApproved = false,
//                    IsDeleted = false
//                },
//                new BaseMessage
//                {
//                    Id = 3,
//                    Description = "GPS Fix",
//                    HexFileName = "gps_fix.hex",
//                    HexFileContent = Convert.FromBase64String("MTAxMDAxMTAxMDAxMDEwMTAxMA=="),
//                    Version = "1.0.5",
//                    CreatedTime = new DateTime(2025, 11, 21, 16, 30, 0),
//                    TopicId = 3,
//                    UploaderId = 3,
//                    PublisherId = 3,
//                    UploadedAt = new DateTime(2025, 11, 21, 16, 30, 0),
//                    IsApproved = true,
//                    IsDeleted = false
//                }
//            );

//            // ----------------------
//            // Diagnostics
//            // ----------------------
//            modelBuilder.Entity<Diagnostic>().HasData(
//                new Diagnostic { Id = 1, Description = "Low battery detected", TopicId = 2, IsSolved = false, SubscriberId = 1, AssignedByAdminId = 1 },
//                new Diagnostic { Id = 2, Description = "GPS signal loss", TopicId = 3, IsSolved = true, SubscriberId = 2, AssignedByAdminId = 2 },
//                new Diagnostic { Id = 3, Description = "Engine temperature high", TopicId = 4, IsSolved = false, SubscriberId = 3, AssignedByAdminId = 3 },
//                new Diagnostic { Id = 4, Description = "Door sensor malfunction", TopicId = 5, IsSolved = true, SubscriberId = 4, AssignedByAdminId = 4 },
//                new Diagnostic { Id = 5, Description = "Security breach attempt", TopicId = 1, IsSolved = false, SubscriberId = 5, AssignedByAdminId = 5 }
//            );

//            // ----------------------
//            // MessageDeliveries
//            // ----------------------
//            modelBuilder.Entity<MessageDelivery>().HasData(
//                new MessageDelivery { Id = 1, MessageId = 1, SubscriberId = 1, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = false },
//                new MessageDelivery { Id = 2, MessageId = 2, SubscriberId = 2, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = true },
//                new MessageDelivery { Id = 3, MessageId = 3, SubscriberId = 3, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = false },
//                new MessageDelivery { Id = 4, MessageId = 2, SubscriberId = 4, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = true },
//                new MessageDelivery { Id = 5, MessageId = 1, SubscriberId = 5, DeliveredAt = new DateTime(2025, 11, 21, 16, 30, 0), IsRead = false }
//            );

//            // ----------------------
//            // Query Filters & Indexes
//            // ----------------------
//            modelBuilder.Entity<BaseMessage>().HasQueryFilter(m => !m.IsDeleted);

//            modelBuilder.Entity<Admin>()
//                .HasIndex(a => a.Email)
//                .IsUnique()
//                .HasFilter("[Email] IS NOT NULL");

//            modelBuilder.Entity<Developer>()
//                .HasIndex(d => d.Email)
//                .IsUnique()
//                .HasFilter("[Email] IS NOT NULL");

//            modelBuilder.Entity<Publisher>()
//                .HasIndex(p => p.Email)
//                .IsUnique()
//                .HasFilter("[Email] IS NOT NULL");

//            modelBuilder.Entity<Subscriber>()
//                .HasIndex(s => s.Email)
//                .IsUnique()
//                .HasFilter("[Email] IS NOT NULL");

//            modelBuilder.Entity<Topic>()
//                .HasQueryFilter(t => !t.IsDeleted)
//                .HasIndex(t => t.Name)
//                .IsUnique();

//            // ----------------------
//            // TPH Inheritance
//            // ----------------------
//            modelBuilder.Entity<BaseMessage>()
//                .HasDiscriminator<string>("MessageType")
//                .HasValue<BaseMessage>("BaseMessage")
//                .HasValue<DiagnosticMessage>("DiagnosticMessage");

//            // ----------------------
//            // Topic ↔ Subscribers Many-to-Many
//            // ----------------------
//            modelBuilder.Entity<Topic>()
//                .HasMany(t => t.Subscribers)
//                .WithMany(s => s.Topics)
//                .UsingEntity(j => j.ToTable("TopicSubscribers"));
//        }
//    }
//}
