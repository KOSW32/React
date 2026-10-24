using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    /// <summary>
    /// Дані для створення або оновлення позиції замовлення
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>Ідентифікатор існуючого замовлення</summary>
        /// <example>1</example>
        [Range(1, int.MaxValue, ErrorMessage = "OrderId має бути додатним числом")]
        public int OrderId { get; set; }

        /// <summary>Ідентифікатор існуючого товару</summary>
        /// <example>2</example>
        [Range(1, int.MaxValue, ErrorMessage = "ProductId має бути додатним числом")]
        public int ProductId { get; set; }

        /// <summary>Кількість одиниць товару (від 1 до 10000)</summary>
        /// <example>2</example>
        [Range(1, 10000, ErrorMessage = "Quantity має бути від 1 до 10000")]
        public int Quantity { get; set; }
    }
}
