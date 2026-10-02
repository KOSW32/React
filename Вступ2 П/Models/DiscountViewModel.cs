using System.ComponentModel.DataAnnotations;

namespace DiscountApp.Models
{
    public class DiscountViewModel
    {
        [Required(ErrorMessage = "Вкажіть ціну товару")]
        [Range(0.01, 1000000, ErrorMessage = "Ціна має бути більшою за нуль")]
        [Display(Name = "Ціна товару, грн")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Вкажіть відсоток знижки")]
        [Range(0, 100, ErrorMessage = "Відсоток знижки має бути від 0 до 100")]
        [Display(Name = "Відсоток знижки, %")]
        public decimal DiscountPercent { get; set; }

        public decimal FinalPrice { get; set; }

        public string? Message { get; set; }
    }
}
