using Microsoft.AspNetCore.Mvc;
using OrdersApi.Models;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> Orders = new()
        {
            new Order { Id = 1, Number = "ORD-001", Total = 1250.50m, CreatedAt = new DateTime(2026, 9, 1, 10, 15, 0) },
            new Order { Id = 2, Number = "ORD-002", Total = 8400m, CreatedAt = new DateTime(2026, 9, 12, 14, 30, 0) },
            new Order { Id = 3, Number = "INV-003", Total = 320m, CreatedAt = new DateTime(2026, 10, 3, 9, 0, 0) }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            return Ok(Orders);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Order> GetById(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);

            if (order is null)
            {
                return NotFound("Order not found");
            }

            return Ok(order);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Order>> Search([FromQuery] string? number, [FromQuery] decimal? minTotal)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return BadRequest("Параметр number є обов'язковим");
            }

            var result = Orders.Where(o =>
                o.Number.Contains(number, StringComparison.OrdinalIgnoreCase));

            if (minTotal.HasValue)
            {
                result = result.Where(o => o.Total >= minTotal.Value);
            }

            return Ok(result.ToList());
        }

        [HttpPost]
        public ActionResult<Order> Create([FromBody] OrderDto dto)
        {
            var order = new Order
            {
                Id = Orders.Count == 0 ? 1 : Orders.Max(o => o.Id) + 1,
                Number = dto.Number,
                Total = dto.Total!.Value,
                CreatedAt = DateTime.Now
            };

            Orders.Add(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Order> Update(int id, [FromBody] OrderDto dto)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);

            if (order is null)
            {
                return NotFound("Order not found");
            }

            order.Number = dto.Number;
            order.Total = dto.Total!.Value;

            return Ok(order);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);

            if (order is null)
            {
                return NotFound("Order not found");
            }

            Orders.Remove(order);

            return NoContent();
        }
    }
}
