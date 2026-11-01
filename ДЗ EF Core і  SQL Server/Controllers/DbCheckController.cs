using AutoMapper;
using ClientManagement.API.Data;
using ClientManagement.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DbCheckController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();

                if (!canConnect)
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                    {
                        connected = false,
                        message = "База даних недоступна. Перевірте рядок підключення та застосуйте міграції."
                    });
                }

                var connection = _context.Database.GetDbConnection();

                return Ok(new
                {
                    connected = true,
                    message = "Підключення до бази даних успішне",
                    provider = _context.Database.ProviderName,
                    server = connection.DataSource,
                    database = connection.Database
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    connected = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("migrations")]
        public async Task<IActionResult> Migrations()
        {
            try
            {
                var applied = await _context.Database.GetAppliedMigrationsAsync();
                var pending = await _context.Database.GetPendingMigrationsAsync();

                return Ok(new
                {
                    applied,
                    pending
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
        }

        [HttpGet("clients")]
        public async Task<IActionResult> Clients()
        {
            try
            {
                var clients = await _context.Clients.AsNoTracking().ToListAsync();

                return Ok(new
                {
                    count = clients.Count,
                    items = _mapper.Map<IEnumerable<ClientReadDto>>(clients)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
            }
        }
    }
}
