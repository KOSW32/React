using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement.API.DTOs
{
    public class InvoiceCreateDto
    {
        [Required(ErrorMessage = "Поле ClientName є обов'язковим")]
        [StringLength(200, ErrorMessage = "ClientName не може бути довшим за 200 символів")]
        public string ClientName { get; set; } = string.Empty;

        [Range(0.01, 1000000000, ErrorMessage = "Total має бути більшим за нуль")]
        public decimal Total { get; set; }
    }
}
