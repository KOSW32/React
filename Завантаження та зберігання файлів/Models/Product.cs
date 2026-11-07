using System.ComponentModel.DataAnnotations;

namespace ProductPhotos.API.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ProductPhoto> Photos { get; set; } = new List<ProductPhoto>();
    }
}
