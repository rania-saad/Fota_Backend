//using Fota.Models;
//using Microsoft.EntityFrameworkCore;

//namespace Fota.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<Topic> Topics { get; set; }
//        public DbSet<Publisher> Publishers { get; set; }
//        public DbSet<Subscriber> Subscribers { get; set; }
//        public DbSet<Message> Messages { get; set; }
//        public DbSet<Subscription> Subscriptions { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // جعل Description اختياري
//            modelBuilder.Entity<Publisher>().Property(p => p.Description).IsRequired(false);
//            modelBuilder.Entity<Subscriber>().Property(s => s.Description).IsRequired(false);

//            // تأمين أن اسم الـ Topic فريد
//            modelBuilder.Entity<Topic>().HasIndex(t => t.Name).IsUnique();

//            // منع تكرار نفس الاشتراك (SubscriberId, TopicId) أكثر من مرة
//            modelBuilder.Entity<Subscription>().HasIndex(s => new { s.SubscriberId, s.TopicId }).IsUnique();

//            base.OnModelCreating(modelBuilder);
//            DbSeeder.Seed(modelBuilder); // 🔥 ناديت الـ Seeder هنا
//        }

       
//    }
//}
