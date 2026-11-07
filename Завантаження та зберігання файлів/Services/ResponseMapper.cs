using ProductPhotos.API.DTOs;
using ProductPhotos.API.Models;

namespace ProductPhotos.API.Services
{
    public static class ResponseMapper
    {
        public static PhotoResponseDto ToResponse(ProductPhoto photo, string baseUrl)
        {
            return new PhotoResponseDto
            {
                Id = photo.Id,
                ProductId = photo.ProductId,
                Url = $"{baseUrl}/uploads/products/{photo.ProductId}/{photo.StoredFileName}",
                FileName = photo.OriginalFileName,
                ContentType = photo.ContentType,
                Size = photo.Size,
                CreatedAt = photo.CreatedAt
            };
        }

        public static ProductResponseDto ToResponse(Product product, string baseUrl)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                Photos = product.Photos
                    .OrderBy(p => p.Id)
                    .Select(p => ToResponse(p, baseUrl))
                    .ToList()
            };
        }
    }
}
