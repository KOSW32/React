using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderItemsApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, 10000)]
        public int Quantity { get; set; }

        [Range(0.01, 1000000)]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
