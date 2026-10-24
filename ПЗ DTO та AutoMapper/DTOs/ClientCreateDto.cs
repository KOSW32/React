using System.ComponentModel.DataAnnotations;

namespace ClientManagementAPI.DTOs
{
    public class ClientCreateDto
    {
        [Required(ErrorMessage = "Поле FullName є обов'язковим")]
        [StringLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Email має некоректний формат")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Phone є обов'язковим")]
        [StringLength(30)]
        public string Phone { get; set; } = string.Empty;
    }
}
