using Microsoft.AspNetCore.Mvc;
using TaskTracker.API.DTOs;
using TaskTracker.API.Models;
using TaskTracker.API.Services;
using TaskStatus = TaskTracker.API.Models.TaskStatus;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemReadDto>>> GetAll(
            [FromQuery] Guid? projectId,
            [FromQuery] TaskStatus? status,
            [FromQuery] TaskPriority? priority)
        {
            return Ok(await _taskService.GetAllAsync(projectId, status, priority));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskItemReadDto>> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task is null)
            {
                return NotFound("Task not found");
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemReadDto>> Create([FromBody] TaskItemCreateDto dto)
        {
            var created = await _taskService.CreateAsync(dto);

            if (created is null)
            {
                return BadRequest("Project not found");
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TaskItemReadDto>> Update(Guid id, [FromBody] TaskItemUpdateDto dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);

            if (updated is null)
            {
                return NotFound("Task not found");
            }

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _taskService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Task not found");
            }

            return NoContent();
        }
    }
}
