using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Models
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Поле Name є обов'язковим")]
        [StringLength(200, ErrorMessage = "Name не може бути довшим за 200 символів")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Price має бути від 0 до 1 000 000")]
        public decimal Price { get; set; }
    }
}
