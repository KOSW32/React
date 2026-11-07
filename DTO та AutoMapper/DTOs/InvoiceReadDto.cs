namespace InvoiceManagement.API.DTOs
{
    public class InvoiceReadDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
