using TaskTracker.API.Models;
using TaskStatus = TaskTracker.API.Models.TaskStatus;

namespace TaskTracker.API.DTOs
{
    public class TaskItemReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
