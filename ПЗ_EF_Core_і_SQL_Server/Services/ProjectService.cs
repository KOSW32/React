using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TaskTracker.API.Data;
using TaskTracker.API.DTOs;
using TaskTracker.API.Models;

namespace TaskTracker.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectReadDto>> GetAllAsync()
        {
            return await _context.Projects
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ProjectTo<ProjectReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ProjectReadDto?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .ProjectTo<ProjectReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<ProjectReadDto> CreateAsync(ProjectCreateDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            project.Id = Guid.NewGuid();
            project.CreatedAt = DateTime.UtcNow;
            project.IsDeleted = false;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectReadDto>(project);
        }

        public async Task<ProjectReadDto?> UpdateAsync(Guid id, ProjectUpdateDto dto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                return null;
            }

            _mapper.Map(dto, project);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                return false;
            }

            project.IsDeleted = true;

            foreach (var task in project.Tasks)
            {
                task.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
