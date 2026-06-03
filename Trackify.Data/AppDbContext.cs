using Microsoft.EntityFrameworkCore;
using Trackify.Core.Models;

namespace Trackify.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TrackedProduct> TrackedProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product entity configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(2048);
                entity.HasIndex(e => e.Url).IsUnique();
                entity.Property(e => e.Source).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CurrentPrice).HasPrecision(18, 2);
                entity.Property(e => e.LowestPrice).HasPrecision(18, 2);
                entity.Property(e => e.HighestPrice).HasPrecision(18, 2);
            });

            // PriceHistory entity configuration
            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(10).HasDefaultValue("VND");
                
                entity.HasIndex(e => new { e.ProductId, e.ScrapedAt }); // Composite index
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.PriceHistories)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete: Delete Product -> Delete PriceHistory
            });

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50).HasDefaultValue("User");
            });

            // TrackedProduct entity configuration
            modelBuilder.Entity<TrackedProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TargetPrice).HasPrecision(18, 2);
                
                // Composite unique index
                entity.HasIndex(e => new { e.UserId, e.ProductId }).IsUnique();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.TrackedProducts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete User -> Delete TrackedProduct

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict); // Maintain safety
            });
        }
    }
}
