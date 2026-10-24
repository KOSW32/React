using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    /// <summary>
    /// Дані для створення або оновлення товару
    /// </summary>
    public class ProductDto
    {
        /// <summary>Назва товару (обов'язкове поле)</summary>
        /// <example>Планшет</example>
        [Required(ErrorMessage = "Поле Name є обов'язковим")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>Ціна товару, грн</summary>
        /// <example>15000</example>
        [Range(0, 1000000, ErrorMessage = "Price має бути від 0 до 1 000 000")]
        public decimal Price { get; set; }
    }
}
