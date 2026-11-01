using System.ComponentModel.DataAnnotations;

namespace ClientManagement.API.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Phone { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
    }
}
