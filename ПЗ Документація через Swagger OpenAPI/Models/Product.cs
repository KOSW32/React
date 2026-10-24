namespace ShopApi.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product
    {
        /// <summary>Унікальний ідентифікатор товару</summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>Назва товару</summary>
        /// <example>Ноутбук</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>Ціна товару, грн</summary>
        /// <example>32000</example>
        public decimal Price { get; set; }
    }
}
