using System.ComponentModel.DataAnnotations;
using TaskTracker.API.Models;
using TaskStatus = TaskTracker.API.Models.TaskStatus;

namespace TaskTracker.API.DTOs
{
    public class TaskItemUpdateDto
    {
        [Required(ErrorMessage = "Поле Title є обов'язковим")]
        [MaxLength(150, ErrorMessage = "Title не може бути довшим за 150 символів")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [EnumDataType(typeof(TaskPriority))]
        public TaskPriority Priority { get; set; }

        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus Status { get; set; }
    }
}
