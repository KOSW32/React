using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskStatus Status { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
