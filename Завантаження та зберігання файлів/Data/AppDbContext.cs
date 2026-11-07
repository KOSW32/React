using Microsoft.EntityFrameworkCore;
using ProductPhotos.API.Models;

namespace ProductPhotos.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductPhoto> ProductPhotos => Set<ProductPhoto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Price).HasPrecision(18, 2);

                entity.HasData(
                    new Product { Id = 1, Name = "Ноутбук", Price = 32000m, CreatedAt = new DateTime(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc) },
                    new Product { Id = 2, Name = "Смартфон", Price = 18500m, CreatedAt = new DateTime(2026, 10, 1, 9, 5, 0, DateTimeKind.Utc) },
                    new Product { Id = 3, Name = "Навушники", Price = 2300m, CreatedAt = new DateTime(2026, 10, 1, 9, 10, 0, DateTimeKind.Utc) }
                );
            });

            modelBuilder.Entity<ProductPhoto>(entity =>
            {
                entity.Property(p => p.StoredFileName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.OriginalFileName).IsRequired().HasMaxLength(255);
                entity.Property(p => p.ContentType).IsRequired().HasMaxLength(100);

                entity.HasOne(p => p.Product)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
