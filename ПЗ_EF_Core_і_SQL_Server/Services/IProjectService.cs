using TaskTracker.API.DTOs;

namespace TaskTracker.API.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectReadDto>> GetAllAsync();
        Task<ProjectReadDto?> GetByIdAsync(Guid id);
        Task<ProjectReadDto> CreateAsync(ProjectCreateDto dto);
        Task<ProjectReadDto?> UpdateAsync(Guid id, ProjectUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
