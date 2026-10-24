namespace ShopApi.Models
{
    /// <summary>
    /// Замовлення
    /// </summary>
    public class Order
    {
        /// <summary>Унікальний ідентифікатор замовлення</summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>Номер замовлення</summary>
        /// <example>ORD-001</example>
        public string Number { get; set; } = string.Empty;

        /// <summary>Дата та час створення</summary>
        /// <example>2026-10-01T10:15:00</example>
        public DateTime CreatedAt { get; set; }
    }
}
