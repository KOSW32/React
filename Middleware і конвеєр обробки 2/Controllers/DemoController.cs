using Microsoft.AspNetCore.Mvc;

namespace BlockedUserMiddleware.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult Profile([FromHeader(Name = "X-User-Id")] Guid? userId)
        {
            return Ok(new
            {
                message = "Доступ дозволено",
                userId = userId?.ToString() ?? "anonymous"
            });
        }
    }
}
