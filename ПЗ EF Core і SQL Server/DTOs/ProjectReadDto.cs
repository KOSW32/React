namespace TaskTracker.API.DTOs
{
    public class ProjectReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TasksCount { get; set; }
    }
}
