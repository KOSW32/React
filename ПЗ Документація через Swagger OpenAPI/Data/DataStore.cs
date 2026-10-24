using ShopApi.Models;

namespace ShopApi.Data
{
    public class DataStore
    {
        public List<Product> Products { get; } = new()
        {
            new Product { Id = 1, Name = "Ноутбук", Price = 32000m },
            new Product { Id = 2, Name = "Смартфон", Price = 18500m },
            new Product { Id = 3, Name = "Навушники", Price = 2300m }
        };

        public List<Order> Orders { get; } = new()
        {
            new Order { Id = 1, Number = "ORD-001", CreatedAt = new DateTime(2026, 10, 1, 10, 15, 0) },
            new Order { Id = 2, Number = "ORD-002", CreatedAt = new DateTime(2026, 10, 5, 14, 30, 0) }
        };

        public List<OrderItem> OrderItems { get; } = new()
        {
            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 32000m },
            new OrderItem { Id = 2, OrderId = 1, ProductId = 3, Quantity = 2, UnitPrice = 2300m },
            new OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 18500m }
        };
    }
}
