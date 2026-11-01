using TaskTracker.API.DTOs;
using TaskTracker.API.Models;
using TaskStatus = TaskTracker.API.Models.TaskStatus;

namespace TaskTracker.API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemReadDto>> GetAllAsync(Guid? projectId, TaskStatus? status, TaskPriority? priority);
        Task<TaskItemReadDto?> GetByIdAsync(Guid id);
        Task<TaskItemReadDto?> CreateAsync(TaskItemCreateDto dto);
        Task<TaskItemReadDto?> UpdateAsync(Guid id, TaskItemUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
