using Microsoft.EntityFrameworkCore;
using OrderItemsApi.Models;

namespace OrderItemsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>().Property(o => o.UnitPrice).HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductName = "Клавіатура", Quantity = 1, UnitPrice = 1200m },
                new OrderItem { Id = 2, OrderId = 1, ProductName = "Миша", Quantity = 2, UnitPrice = 450m },
                new OrderItem { Id = 3, OrderId = 2, ProductName = "Монітор", Quantity = 1, UnitPrice = 6500m }
            );
        }
    }
}
