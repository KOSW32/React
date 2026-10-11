using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Models
{
    public class OrderDto
    {
        [Required(ErrorMessage = "Поле Number є обов'язковим")]
        [StringLength(50, ErrorMessage = "Number не може бути довшим за 50 символів")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Total є обов'язковим")]
        [Range(0.01, 100000000, ErrorMessage = "Total має бути більшим за нуль")]
        public decimal? Total { get; set; }
    }
}
