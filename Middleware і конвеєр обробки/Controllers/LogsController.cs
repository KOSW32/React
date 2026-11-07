using Microsoft.AspNetCore.Mvc;
using RequestLogger.API.Logging;

namespace RequestLogger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IFileLogger _logger;

        public LogsController(IFileLogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetLast([FromQuery] int count = 20)
        {
            if (count < 1 || count > 200)
            {
                return BadRequest("count має бути від 1 до 200");
            }

            return Ok(await _logger.ReadLastAsync(count));
        }
    }
}
