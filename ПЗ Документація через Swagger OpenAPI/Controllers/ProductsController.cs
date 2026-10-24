using Microsoft.AspNetCore.Mvc;
using ShopApi.Data;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    /// <summary>
    /// Керування товарами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly DataStore _store;

        public ProductsController(DataStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Отримати список усіх товарів
        /// </summary>
        /// <returns>Список товарів</returns>
        /// <response code="200">Повертає список товарів</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(_store.Products);
        }

        /// <summary>
        /// Отримати товар за ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор товару</param>
        /// <response code="200">Товар знайдено</response>
        /// <response code="404">Товар не знайдено</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetById(int id)
        {
            var product = _store.Products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }

        /// <summary>
        /// Знайти товари за частиною назви
        /// </summary>
        /// <param name="name">Частина назви (без урахування регістру)</param>
        /// <response code="200">Повертає список знайдених товарів (може бути порожнім)</response>
        /// <response code="400">Параметр name не вказано</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Product>> Search([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Параметр name є обов'язковим");
            }

            var result = _store.Products
                .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Створити новий товар
        /// </summary>
        /// <param name="dto">Дані товару</param>
        /// <response code="201">Товар створено</response>
        /// <response code="400">Некоректні дані</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> Create([FromBody] ProductDto dto)
        {
            var product = new Product
            {
                Id = _store.Products.Count == 0 ? 1 : _store.Products.Max(p => p.Id) + 1,
                Name = dto.Name,
                Price = dto.Price
            };

            _store.Products.Add(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Оновити товар
        /// </summary>
        /// <param name="id">Ідентифікатор товару</param>
        /// <param name="dto">Нові дані товару</param>
        /// <response code="200">Товар оновлено</response>
        /// <response code="400">Некоректні дані</response>
        /// <response code="404">Товар не знайдено</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult<Product> Update(int id, [FromBody] ProductDto dto)
        {
            var product = _store.Products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            product.Name = dto.Name;
            product.Price = dto.Price;

            return Ok(product);
        }

        /// <summary>
        /// Видалити товар
        /// </summary>
        /// <param name="id">Ідентифікатор товару</param>
        /// <response code="204">Товар видалено</response>
        /// <response code="404">Товар не знайдено</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var product = _store.Products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            _store.Products.Remove(product);

            return NoContent();
        }
    }
}
