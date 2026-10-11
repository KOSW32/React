using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderItemsApi.Data;
using OrderItemsApi.Models;

namespace OrderItemsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAll()
        {
            var items = await _context.OrderItems.AsNoTracking().ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderItem>> GetById(int id)
        {
            var item = await _context.OrderItems.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (item is null)
            {
                return NotFound("OrderItem not found");
            }

            return Ok(item);
        }

        [HttpGet("by-order/{orderId:int}")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetByOrder(int orderId)
        {
            var items = await _context.OrderItems
                .AsNoTracking()
                .Where(o => o.OrderId == orderId)
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> Create([FromBody] OrderItem item)
        {
            item.Id = 0;

            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrderItem>> Update(int id, [FromBody] OrderItem updated)
        {
            var item = await _context.OrderItems.FirstOrDefaultAsync(o => o.Id == id);

            if (item is null)
            {
                return NotFound("OrderItem not found");
            }

            item.OrderId = updated.OrderId;
            item.ProductName = updated.ProductName;
            item.Quantity = updated.Quantity;
            item.UnitPrice = updated.UnitPrice;

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.OrderItems.FirstOrDefaultAsync(o => o.Id == id);

            if (item is null)
            {
                return NotFound("OrderItem not found");
            }

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
