using Microsoft.AspNetCore.Mvc;
using OrdersPaging.API.Models;

namespace OrdersPaging.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static readonly string[] SortFields = { "id", "number", "total", "createdAt" };

        private static readonly List<Order> Orders = Enumerable.Range(1, 15)
            .Select(i => new Order
            {
                Id = i,
                Number = $"{(i % 4 == 0 ? "INV" : "ORD")}-{i:D3}",
                Total = 300m + (i * 7919 % 9000),
                CreatedAt = new DateTime(2026, 9, 1, 9, 0, 0).AddDays(i * 2).AddHours(i % 5)
            })
            .ToList();

        [HttpGet]
        public ActionResult<PagedResult<Order>> GetAll([FromQuery] OrderQueryParameters query)
        {
            var sortBy = SortFields.FirstOrDefault(f => f.Equals(query.SortBy, StringComparison.OrdinalIgnoreCase));

            if (sortBy is null)
            {
                return BadRequest($"Невідоме поле сортування. Доступні значення: {string.Join(", ", SortFields)}");
            }

            var isAscending = query.SortDir.Equals("asc", StringComparison.OrdinalIgnoreCase);
            var isDescending = query.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase);

            if (!isAscending && !isDescending)
            {
                return BadRequest("SortDir має бути asc або desc");
            }

            if (query.MinTotal.HasValue && query.MaxTotal.HasValue && query.MinTotal > query.MaxTotal)
            {
                return BadRequest("MinTotal не може бути більшим за MaxTotal");
            }

            if (query.CreatedFrom.HasValue && query.CreatedTo.HasValue && query.CreatedFrom > query.CreatedTo)
            {
                return BadRequest("CreatedFrom не може бути пізніше за CreatedTo");
            }

            IQueryable<Order> orders = Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Number))
            {
                orders = orders.Where(o => o.Number.Contains(query.Number, StringComparison.OrdinalIgnoreCase));
            }

            if (query.MinTotal.HasValue)
            {
                orders = orders.Where(o => o.Total >= query.MinTotal.Value);
            }

            if (query.MaxTotal.HasValue)
            {
                orders = orders.Where(o => o.Total <= query.MaxTotal.Value);
            }

            if (query.CreatedFrom.HasValue)
            {
                orders = orders.Where(o => o.CreatedAt >= query.CreatedFrom.Value);
            }

            if (query.CreatedTo.HasValue)
            {
                var createdTo = query.CreatedTo.Value;

                if (createdTo.TimeOfDay == TimeSpan.Zero)
                {
                    var nextDay = createdTo.AddDays(1);
                    orders = orders.Where(o => o.CreatedAt < nextDay);
                }
                else
                {
                    orders = orders.Where(o => o.CreatedAt <= createdTo);
                }
            }

            var sorted = ApplySorting(orders, sortBy, isDescending).ThenBy(o => o.Id);

            var totalCount = sorted.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

            var items = sorted
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return Ok(new PagedResult<Order>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
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

        private static IOrderedQueryable<Order> ApplySorting(IQueryable<Order> source, string sortBy, bool descending)
        {
            return sortBy switch
            {
                "number" => descending ? source.OrderByDescending(o => o.Number) : source.OrderBy(o => o.Number),
                "total" => descending ? source.OrderByDescending(o => o.Total) : source.OrderBy(o => o.Total),
                "createdAt" => descending ? source.OrderByDescending(o => o.CreatedAt) : source.OrderBy(o => o.CreatedAt),
                _ => descending ? source.OrderByDescending(o => o.Id) : source.OrderBy(o => o.Id)
            };
        }
    }
}
