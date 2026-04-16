using Entities;
using Microsoft.EntityFrameworkCore;
using OfisUrunTakip.WebApi.Entity;

namespace OfisUrunTakip.WebApi.Data
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=192.168.2.240;Initial Catalog=ApiOfisDb;User ID=sa;Password=My6031*+;TrustServerCertificate=True;Persist Security Info=True;Encrypt=False;Connection Timeout=60;");
        } //context nesnesi ile ilgili ayarlamalarımızı yapmöamızı saglayan temel metedomuzdur  yanı tabloyunu nerden olusturacagını falan yapar 

        public DbSet<Category> Categories { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<EmailNotificationSetting> EmailNotificationSettings { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product → Category ilişkisi, cascade delete engellendi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // veya SetNull

            modelBuilder.Entity<UserEmailSetting>()
                .HasOne(u => u.User)
                .WithOne() //based on the logic that every user has a setting
                .HasForeignKey<UserEmailSetting>(u => u.UserId);

            modelBuilder.Entity<User>()
            .HasOne(u => u.EmailSetting)      // Bir User'ın bir EmailSetting'i vardır.
            .WithOne(es => es.User)           // Bir EmailSetting bir User'a aittir.
            .HasForeignKey<UserEmailSetting>(es => es.UserId); // FK, UserEmailSetting tablosundadır.


            modelBuilder.Entity<Product>()
          .HasIndex(p => p.Name)
          .IsUnique()
          .HasFilter("[IsDeleted] = 0");


        }

        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEmailSetting> UserEmailSettings { get; set; }

        //DbSet < entitydeki olan classın adı> sqlde oluşmasını istediğimiz  tablosunun adı  böyle kullanılıyor 
    }
}