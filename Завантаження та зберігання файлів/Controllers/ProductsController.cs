using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductPhotos.API.Data;
using ProductPhotos.API.DTOs;
using ProductPhotos.API.Models;
using ProductPhotos.API.Services;

namespace ProductPhotos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;

        public ProductsController(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        private string BaseUrl => $"{Request.Scheme}://{Request.Host}";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Photos)
                .OrderBy(p => p.Id)
                .ToListAsync();

            return Ok(products.Select(p => ResponseMapper.ToResponse(p, BaseUrl)));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return Ok(ResponseMapper.ToResponse(product, BaseUrl));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductResponseDto>> Create([FromForm] ProductCreateDto dto)
        {
            var photos = dto.Photos ?? new List<IFormFile>();

            if (photos.Count > 0)
            {
                var error = _photoService.Validate(photos);

                if (error is not null)
                {
                    return BadRequest(error);
                }
            }

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (photos.Count > 0)
            {
                try
                {
                    var saved = await _photoService.SaveAsync(product.Id, photos);
                    _context.ProductPhotos.AddRange(saved);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    _photoService.DeleteProductFolder(product.Id);
                    _context.ChangeTracker.Clear();
                    _context.Products.Remove(new Product { Id = product.Id });
                    await _context.SaveChangesAsync();
                    throw;
                }
            }

            var created = await _context.Products
                .AsNoTracking()
                .Include(p => p.Photos)
                .FirstAsync(p => p.Id == product.Id);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ResponseMapper.ToResponse(created, BaseUrl));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            var product = await _context.Products
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            product.Name = dto.Name;
            product.Price = dto.Price;
            await _context.SaveChangesAsync();

            return Ok(ResponseMapper.ToResponse(product, BaseUrl));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _photoService.DeleteProductFolder(id);

            return NoContent();
        }
    }
}
