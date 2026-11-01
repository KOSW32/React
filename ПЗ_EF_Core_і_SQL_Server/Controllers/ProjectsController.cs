using Microsoft.AspNetCore.Mvc;
using TaskTracker.API.DTOs;
using TaskTracker.API.Services;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;

        public ProjectsController(IProjectService projectService, ITaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetAll()
        {
            return Ok(await _projectService.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProjectReadDto>> GetById(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);

            if (project is null)
            {
                return NotFound("Project not found");
            }

            return Ok(project);
        }

        [HttpGet("{id:guid}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItemReadDto>>> GetTasks(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);

            if (project is null)
            {
                return NotFound("Project not found");
            }

            return Ok(await _taskService.GetAllAsync(id, null, null));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectReadDto>> Create([FromBody] ProjectCreateDto dto)
        {
            var created = await _projectService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProjectReadDto>> Update(Guid id, [FromBody] ProjectUpdateDto dto)
        {
            var updated = await _projectService.UpdateAsync(id, dto);

            if (updated is null)
            {
                return NotFound("Project not found");
            }

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _projectService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Project not found");
            }

            return NoContent();
        }
    }
}
