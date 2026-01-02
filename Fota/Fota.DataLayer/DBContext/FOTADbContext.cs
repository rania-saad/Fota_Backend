using Fota.DataLayer.Models;
using Fota.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Fota.DataLayer.Enum;

namespace Fota.DataLayer.DBContext
{
    public partial class FOTADbContext : IdentityDbContext<ApplicationUser>
    {
        public FOTADbContext(DbContextOptions<FOTADbContext> options)
            : base(options) { }

        // DbSetsa
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamDeveloper> TeamDevelopers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TeamTopic> TeamTopics { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<TopicSubscriber> TopicSubscribers { get; set; }
        public DbSet<BaseMessage> BaseMessages { get; set; }
        public DbSet<MessageDelivery> MessageDeliveries { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }
        public DbSet<DiagnosticSolution> DiagnosticSolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global: Disable cascade delete for all foreign keys
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
                }
            }

            // ==================== CONFIGURATIONS ====================

            // Query Filters for Soft Delete
            modelBuilder.Entity<BaseMessage>()
                .HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.Entity<Topic>()
                .HasQueryFilter(t => !t.IsDeleted);

            // Unique Indexes
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<Developer>()
                .HasIndex(d => d.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<Subscriber>()
                .HasIndex(s => s.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<Topic>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // ==================== RELATIONSHIPS ====================

            // Team -> Developer (Lead)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Lead)
                .WithMany(d => d.LeadingTeams)
                .HasForeignKey(t => t.LeadId)
                .OnDelete(DeleteBehavior.NoAction);

            // Team -> Admin (Creator)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.CreatedByAdmin)
                .WithMany(a => a.CreatedTeams)
                .HasForeignKey(t => t.CreatedByAdminId)
                .OnDelete(DeleteBehavior.NoAction);

            // BaseMessage relationships
            modelBuilder.Entity<BaseMessage>()
                .HasOne(m => m.Uploader)
                .WithMany(d => d.UploadedMessages)
                .HasForeignKey(m => m.UploaderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BaseMessage>()
                .HasOne(m => m.Publisher)
                .WithMany(d => d.PublishedMessages)
                .HasForeignKey(m => m.PublisherId)
                .OnDelete(DeleteBehavior.NoAction);

            // Diagnostic relationships
            modelBuilder.Entity<Diagnostic>()
                .HasOne(d => d.AssignedByAdmin)
                .WithMany(a => a.AssignedDiagnostics)
                .HasForeignKey(d => d.AssignedByAdminId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Diagnostic>()
                .HasOne(d => d.AssignedToDeveloper)
                .WithMany(dev => dev.AssignedDiagnostics)
                .HasForeignKey(d => d.AssignedToDeveloperId)
                .OnDelete(DeleteBehavior.NoAction);

            // ==================== SEED DATA ====================

            // Admins - Realistic data
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Name = "Sarah Johnson",
                    Email = "sarah.johnson@fotasystem.com",
                    PhoneNumber = "+201234567890",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 15, 10, 0, 0)
                },
                new Admin
                {
                    Id = 2,
                    Name = "Ahmed Hassan",
                    Email = "ahmed.hassan@fotasystem.com",
                    PhoneNumber = "+201234567891",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 15, 10, 0, 0)
                }
            );

            // Developers - Realistic data
            modelBuilder.Entity<Developer>().HasData(
                new Developer
                {
                    Id = 1,
                    Name = "Mohamed Ali",
                    Email = "mohamed.ali@fotasystem.com",
                    PhoneNumber = "+201234567892",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 2, 1, 9, 0, 0)
                },
                new Developer
                {
                    Id = 2,
                    Name = "Fatima Nour",
                    Email = "fatima.nour@fotasystem.com",
                    PhoneNumber = "+201234567893",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 2, 1, 9, 0, 0)
                },
                new Developer
                {
                    Id = 3,
                    Name = "Omar Khaled",
                    Email = "omar.khaled@fotasystem.com",
                    PhoneNumber = "+201234567894",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 2, 5, 9, 0, 0)
                }
            );

            // Topics - Realistic automotive domains
            modelBuilder.Entity<Topic>().HasData(
                new Topic
                {
                    Id = 1,
                    Name = "Engine Control Unit",
                    Description = "ECU firmware updates and diagnostics",
                    IsDeleted = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 1, 8, 0, 0)
                },
                new Topic
                {
                    Id = 2,
                    Name = "Battery Management System",
                    Description = "BMS firmware for electric vehicles",
                    IsDeleted = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 1, 8, 0, 0)
                },
                new Topic
                {
                    Id = 3,
                    Name = "Infotainment System",
                    Description = "Navigation and multimedia updates",
                    IsDeleted = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 1, 8, 0, 0)
                }
            );

            // Teams
            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    Name = "ECU Development Team",
                    Description = "Responsible for engine control updates",
                    LeadId = 1,
                    CreatedByAdminId = 1,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 10, 10, 0, 0)
                },
                new Team
                {
                    Id = 2,
                    Name = "Battery Systems Team",
                    Description = "Handles BMS firmware development",
                    LeadId = 2,
                    CreatedByAdminId = 1,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 10, 10, 0, 0)
                }
            );

            // TeamTopics - Linking teams to their topics
            modelBuilder.Entity<TeamTopic>().HasData(
                new TeamTopic
                {
                    Id = 1,
                    TeamId = 1,
                    TopicId = 1,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 10, 10, 30, 0)
                },
                new TeamTopic
                {
                    Id = 2,
                    TeamId = 2,
                    TopicId = 2,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 10, 10, 30, 0)
                }
            );

            // TeamDevelopers - Team membership
            modelBuilder.Entity<TeamDeveloper>().HasData(
                new TeamDeveloper
                {
                    Id = 1,
                    TeamId = 1,
                    DeveloperId = 1,
                    Role = "Lead",
                    JoinedAt = new DateTime(2024, 3, 10, 10, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 10, 10, 0, 0)
                },
                new TeamDeveloper
                {
                    Id = 2,
                    TeamId = 1,
                    DeveloperId = 3,
                    Role = "Senior",
                    JoinedAt = new DateTime(2024, 3, 12, 9, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 12, 9, 0, 0)
                },
                new TeamDeveloper
                {
                    Id = 3,
                    TeamId = 2,
                    DeveloperId = 2,
                    Role = "Lead",
                    JoinedAt = new DateTime(2024, 3, 10, 10, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 3, 10, 10, 0, 0)
                }
            );

            // Subscribers - Vehicle owners/testers
            modelBuilder.Entity<Subscriber>().HasData(
                new Subscriber
                {
                    Id = 1,
                    Name = "Toyota Fleet - Cairo",
                    Email = "fleet.cairo@toyota-eg.com",
                    PhoneNumber = "+201234567895",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 4, 1, 8, 0, 0)
                },
                new Subscriber
                {
                    Id = 2,
                    Name = "BMW Test Center",
                    Email = "test.center@bmw-eg.com",
                    PhoneNumber = "+201234567896",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 4, 1, 8, 0, 0)
                }
            );

            // TopicSubscribers - Subscriptions
            modelBuilder.Entity<TopicSubscriber>().HasData(
                new TopicSubscriber
                {
                    Id = 1,
                    TopicId = 1,
                    SubscriberId = 1,
                    SubscribedAt = new DateTime(2024, 4, 5, 9, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 4, 5, 9, 0, 0)
                },
                new TopicSubscriber
                {
                    Id = 2,
                    TopicId = 2,
                    SubscriberId = 2,
                    SubscribedAt = new DateTime(2024, 4, 5, 9, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 4, 5, 9, 0, 0)
                }
            );

            // BaseMessages - Firmware updates
            modelBuilder.Entity<BaseMessage>().HasData(
                new BaseMessage
                {
                    Id = 1,
                    MessageType = BaseMessageType.Standard,
                    Description = "ECU Firmware Update v2.3.1 - Performance improvements",
                    HexFileName = "ecu_v2.3.1.hex",
                    Version = "2.3.1",
                    Status = BaseMessageStatus.Published,
                    TopicId = 1,
                    UploaderId = 1,
                    PublisherId = 1,
                    PublishedAt = new DateTime(2024, 5, 10, 14, 30, 0),
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 5, 10, 10, 0, 0)
                },
                new BaseMessage
                {
                    Id = 2,
                    MessageType = BaseMessageType.Standard,
                    Description = "BMS Critical Security Patch v1.8.2",
                    HexFileName = "bms_v1.8.2_security.hex",
                    Version = "1.8.2",
                    Status = BaseMessageStatus.Approved,
                    TopicId = 2,
                    UploaderId = 2,
                    ApprovedAt = new DateTime(2024, 5, 15, 11, 0, 0),
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 5, 15, 9, 0, 0)
                }
            );

            // MessageDeliveries
            modelBuilder.Entity<MessageDelivery>().HasData(
                new MessageDelivery
                {
                    Id = 1,
                    MessageId = 1,
                    SubscriberId = 1,
                    DeliveredAt = new DateTime(2024, 5, 10, 15, 0, 0),
                    IsDelivered = true,
                    IsRead = true,
                    ReadAt = new DateTime(2024, 5, 10, 16, 30, 0),
                    IsFailed = false,
                    CreatedAt = new DateTime(2024, 5, 10, 14, 30, 0)
                }
            );

            // Diagnostics
            modelBuilder.Entity<Diagnostic>().HasData(
                new Diagnostic
                {
                    Id = 1,
                    Title = "Battery Temperature Warning",
                    Description = "Battery pack temperature exceeding normal range in test vehicle VIN:ABC123",
                    Priority = DiagnosticPriority.High,
                    Status = DiagnosticStatus.InProgress,
                    TopicId = 2,
                    SubscriberId = 2,
                    AssignedByAdminId = 1,
                    AssignedToDeveloperId = 2,
                    CreatedAt = new DateTime(2024, 5, 20, 8, 30, 0)
                }
            );
        }
    }
}