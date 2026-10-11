using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products = new()
        {
            new Product { Id = 1, Name = "Ноутбук", Price = 32000m },
            new Product { Id = 2, Name = "Смартфон", Price = 18500m },
            new Product { Id = 3, Name = "Навушники", Price = 2300m }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(Products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Product>> Search([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Параметр name є обов'язковим");
            }

            var result = Products
                .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] ProductDto dto)
        {
            var product = new Product
            {
                Id = Products.Count == 0 ? 1 : Products.Max(p => p.Id) + 1,
                Name = dto.Name,
                Price = dto.Price
            };

            Products.Add(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Product> Update(int id, [FromBody] ProductDto dto)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            product.Name = dto.Name;
            product.Price = dto.Price;

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            Products.Remove(product);

            return NoContent();
        }
    }
}
