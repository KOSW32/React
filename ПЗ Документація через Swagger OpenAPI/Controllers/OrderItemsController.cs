using Microsoft.AspNetCore.Mvc;
using ShopApi.Data;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Керування позиціями замовлень
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrderItemsController : ControllerBase
    {
        private readonly DataStore _store;

        public OrderItemsController(DataStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Отримати список усіх позицій замовлень
        /// </summary>
        /// <response code="200">Повертає список позицій</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderItem>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderItem>> GetAll()
        {
            return Ok(_store.OrderItems);
        }

        /// <summary>
        /// Отримати позицію за ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор позиції</param>
        /// <response code="200">Позицію знайдено</response>
        /// <response code="404">Позицію не знайдено</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderItem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<OrderItem> GetById(int id)
        {
            var item = _store.OrderItems.FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                return NotFound("OrderItem not found");
            }

            return Ok(item);
        }

        /// <summary>
        /// Додати позицію до замовлення
        /// </summary>
        /// <remarks>
        /// Ціна за одиницю береться з товару на момент створення позиції.
        ///
        /// Приклад запиту:
        ///
        ///     POST /api/orderitems
        ///     {
        ///        "orderId": 1,
        ///        "productId": 2,
        ///        "quantity": 3
        ///     }
        /// </remarks>
        /// <param name="dto">Дані позиції</param>
        /// <response code="201">Позицію створено</response>
        /// <response code="400">Некоректні дані або не існує замовлення чи товар</response>
        [HttpPost]
        [ProducesResponseType(typeof(OrderItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrderItem> Create([FromBody] OrderItemDto dto)
        {
            if (!_store.Orders.Any(o => o.Id == dto.OrderId))
            {
                return BadRequest("Order not found");
            }

            var product = _store.Products.FirstOrDefault(p => p.Id == dto.ProductId);

            if (product is null)
            {
                return BadRequest("Product not found");
            }

            var item = new OrderItem
            {
                Id = _store.OrderItems.Count == 0 ? 1 : _store.OrderItems.Max(i => i.Id) + 1,
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.Price
            };

            _store.OrderItems.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        /// <summary>
        /// Оновити позицію замовлення
        /// </summary>
        /// <param name="id">Ідентифікатор позиції</param>
        /// <param name="dto">Нові дані позиції</param>
        /// <response code="200">Позицію оновлено</response>
        /// <response code="400">Некоректні дані або не існує замовлення чи товар</response>
        /// <response code="404">Позицію не знайдено</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<OrderItem> Update(int id, [FromBody] OrderItemDto dto)
        {
            var item = _store.OrderItems.FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                return NotFound("OrderItem not found");
            }

            if (!_store.Orders.Any(o => o.Id == dto.OrderId))
            {
                return BadRequest("Order not found");
            }

            var product = _store.Products.FirstOrDefault(p => p.Id == dto.ProductId);

            if (product is null)
            {
                return BadRequest("Product not found");
            }

            item.OrderId = dto.OrderId;
            item.ProductId = dto.ProductId;
            item.Quantity = dto.Quantity;
            item.UnitPrice = product.Price;

            return Ok(item);
        }

        /// <summary>
        /// Видалити позицію замовлення
        /// </summary>
        /// <param name="id">Ідентифікатор позиції</param>
        /// <response code="204">Позицію видалено</response>
        /// <response code="404">Позицію не знайдено</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var item = _store.OrderItems.FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                return NotFound("OrderItem not found");
            }

            _store.OrderItems.Remove(item);

            return NoContent();
        }
    }
}
