namespace ShopApi.Models
{
    /// <summary>
    /// Позиція замовлення (товар у замовленні)
    /// </summary>
    public class OrderItem
    {
        /// <summary>Унікальний ідентифікатор позиції</summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>Ідентифікатор замовлення</summary>
        /// <example>1</example>
        public int OrderId { get; set; }

        /// <summary>Ідентифікатор товару</summary>
        /// <example>2</example>
        public int ProductId { get; set; }

        /// <summary>Кількість одиниць товару</summary>
        /// <example>3</example>
        public int Quantity { get; set; }

        /// <summary>Ціна за одиницю на момент створення позиції, грн</summary>
        /// <example>18500</example>
        public decimal UnitPrice { get; set; }

        /// <summary>Сума за позицію (кількість × ціна)</summary>
        /// <example>55500</example>
        public decimal Total => Quantity * UnitPrice;
    }
}
