using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductPhotos.API.Data;
using ProductPhotos.API.DTOs;
using ProductPhotos.API.Services;

namespace ProductPhotos.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId:int}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;

        public PhotosController(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        private string BaseUrl => $"{Request.Scheme}://{Request.Host}";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotoResponseDto>>> GetAll(int productId)
        {
            if (!await _context.Products.AnyAsync(p => p.Id == productId))
            {
                return NotFound("Product not found");
            }

            var photos = await _context.ProductPhotos
                .AsNoTracking()
                .Where(p => p.ProductId == productId)
                .OrderBy(p => p.Id)
                .ToListAsync();

            return Ok(photos.Select(p => ResponseMapper.ToResponse(p, BaseUrl)));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<IEnumerable<PhotoResponseDto>>> Upload(int productId, [FromForm] List<IFormFile> files)
        {
            if (!await _context.Products.AnyAsync(p => p.Id == productId))
            {
                return NotFound("Product not found");
            }

            if (files.Count == 0)
            {
                return BadRequest("Додайте хоча б один файл");
            }

            var error = _photoService.Validate(files);

            if (error is not null)
            {
                return BadRequest(error);
            }

            var existingCount = await _context.ProductPhotos.CountAsync(p => p.ProductId == productId);

            if (existingCount + files.Count > PhotoRules.MaxPhotosPerProduct)
            {
                return BadRequest($"Для одного товару можна мати не більше {PhotoRules.MaxPhotosPerProduct} фотографій. Зараз їх {existingCount}");
            }

            var saved = await _photoService.SaveAsync(productId, files);

            _context.ProductPhotos.AddRange(saved);
            await _context.SaveChangesAsync();

            var result = saved.Select(p => ResponseMapper.ToResponse(p, BaseUrl)).ToList();

            return CreatedAtAction(nameof(GetAll), new { productId }, result);
        }

        [HttpDelete("{photoId:int}")]
        public async Task<IActionResult> Delete(int productId, int photoId)
        {
            var photo = await _context.ProductPhotos
                .FirstOrDefaultAsync(p => p.Id == photoId && p.ProductId == productId);

            if (photo is null)
            {
                return NotFound("Photo not found");
            }

            _context.ProductPhotos.Remove(photo);
            await _context.SaveChangesAsync();

            _photoService.DeleteFile(photo);

            return NoContent();
        }
    }
}
