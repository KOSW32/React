using Microsoft.AspNetCore.Mvc;
using ShopApi.Data;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Керування замовленнями
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly DataStore _store;

        public OrdersController(DataStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Отримати список усіх замовлень
        /// </summary>
        /// <response code="200">Повертає список замовлень</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            return Ok(_store.Orders);
        }

        /// <summary>
        /// Отримати замовлення за ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор замовлення</param>
        /// <response code="200">Замовлення знайдено</response>
        /// <response code="404">Замовлення не знайдено</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<Order> GetById(int id)
        {
            var order = _store.Orders.FirstOrDefault(o => o.Id == id);

            if (order is null)
            {
                return NotFound("Order not found");
            }

            return Ok(order);
        }

        /// <summary>
        /// Отримати позиції конкретного замовлення
        /// </summary>
        /// <param name="id">Ідентифікатор замовлення</param>
        /// <response code="200">Повертає позиції замовлення (може бути порожній список)</response>
        /// <response code="404">Замовлення не знайдено</response>
        [HttpGet("{id:int}/items")]
        [ProducesResponseType(typeof(IEnumerable<OrderItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderItem>> GetItems(int id)
        {
            if (!_store.Orders.Any(o => o.Id == id))
            {
                return NotFound("Order not found");
            }

            var items = _store.OrderItems.Where(i => i.OrderId == id).ToList();

            return Ok(items);
        }

        /// <summary>
        /// Створити нове замовлення
        /// </summary>
        /// <param name="dto">Дані замовлення</param>
        /// <response code="201">Замовлення створено</response>
        /// <response code="400">Некоректні дані</response>
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Order> Create([FromBody] OrderDto dto)
        {
            var order = new Order
            {
                Id = _store.Orders.Count == 0 ? 1 : _store.Orders.Max(o => o.Id) + 1,
                Number = dto.Number,
                CreatedAt = DateTime.Now
            };

            _store.Orders.Add(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>
        /// Оновити замовлення
        /// </summary>
        /// <param name="id">Ідентифікатор замовлення</param>
        /// <param name="dto">Нові дані замовлення</param>
        /// <response code="200">Замовлення оновлено</response>
        /// <response code="400">Некоректні дані</response>
        /// <response code="404">Замовлення не знайдено</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<Order> Update(int id, [FromBody] OrderDto dto)
        {
            var order = _store.Orders.FirstOrDefault(o => o.Id == id);

            if (order is null)
            {
                return NotFound("Order not found");
            }

            order.Number = dto.Number;

            return Ok(order);
        }

        /// <summary>
        /// Видалити замовлення разом з його позиціями
        /// </summary>
        /// <param name="id">Ідентифікатор замовлення</param>
        /// <response code="204">Замовлення видалено</response>
        /// <response code="404">Замовлення не знайдено</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var order = _store.Orders.FirstOrDefault(o => o.Id == id);

            if (order is null)
            {
                return NotFound("Order not found");
            }

            _store.OrderItems.RemoveAll(i => i.OrderId == id);
            _store.Orders.Remove(order);

            return NoContent();
        }
    }
}
