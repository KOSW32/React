using Microsoft.AspNetCore.Mvc;

namespace RequestLogger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        [HttpGet("ok")]
        public IActionResult Ok200()
        {
            return Ok(new { message = "Усе працює" });
        }

        [HttpGet("not-found")]
        public IActionResult NotFound404()
        {
            return NotFound("Ресурс не знайдено");
        }

        [HttpGet("bad-request")]
        public IActionResult BadRequest400()
        {
            return BadRequest("Некоректний запит");
        }

        [HttpGet("slow")]
        public async Task<IActionResult> Slow()
        {
            await Task.Delay(700);
            return Ok(new { message = "Повільна відповідь" });
        }

        [HttpGet("error")]
        public IActionResult Error500()
        {
            throw new InvalidOperationException("Тестова помилка для логера");
        }
    }
}
