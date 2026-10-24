using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    /// <summary>
    /// Дані для створення або оновлення замовлення
    /// </summary>
    public class OrderDto
    {
        /// <summary>Номер замовлення (обов'язкове поле)</summary>
        /// <example>ORD-004</example>
        [Required(ErrorMessage = "Поле Number є обов'язковим")]
        [StringLength(50)]
        public string Number { get; set; } = string.Empty;
    }
}
