using ProductPhotos.API.Models;

namespace ProductPhotos.API.Services
{
    public static class PhotoRules
    {
        public const long MaxFileSize = 5 * 1024 * 1024;
        public const int MaxFilesPerRequest = 5;
        public const int MaxPhotosPerProduct = 10;
        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    }

    public interface IPhotoService
    {
        string? Validate(IReadOnlyCollection<IFormFile> files);
        Task<List<ProductPhoto>> SaveAsync(int productId, IReadOnlyCollection<IFormFile> files);
        void DeleteFile(ProductPhoto photo);
        void DeleteProductFolder(int productId);
    }
}
