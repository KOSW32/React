using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TaskTracker.API.Data;
using TaskTracker.API.DTOs;
using TaskTracker.API.Models;
using TaskStatus = TaskTracker.API.Models.TaskStatus;

namespace TaskTracker.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItemReadDto>> GetAllAsync(Guid? projectId, TaskStatus? status, TaskPriority? priority)
        {
            var query = _context.Tasks.AsNoTracking().AsQueryable();

            if (projectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ProjectTo<TaskItemReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TaskItemReadDto?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<TaskItemReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<TaskItemReadDto?> CreateAsync(TaskItemCreateDto dto)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);

            if (!projectExists)
            {
                return null;
            }

            var task = _mapper.Map<TaskItem>(dto);
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.IsDeleted = false;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(task.Id);
        }

        public async Task<TaskItemReadDto?> UpdateAsync(Guid id, TaskItemUpdateDto dto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task is null)
            {
                return null;
            }

            _mapper.Map(dto, task);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task is null)
            {
                return false;
            }

            task.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
