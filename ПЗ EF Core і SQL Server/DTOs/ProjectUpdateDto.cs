using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs
{
    public class ProjectUpdateDto
    {
        [Required(ErrorMessage = "Поле Name є обов'язковим")]
        [MaxLength(100, ErrorMessage = "Name не може бути довшим за 100 символів")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
