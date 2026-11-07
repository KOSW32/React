using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement.API.DTOs
{
    public class InvoiceUpdateDto
    {
        [Range(0.01, 1000000000, ErrorMessage = "Total має бути більшим за нуль")]
        public decimal Total { get; set; }
    }
}
