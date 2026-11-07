using ProductPhotos.API.Models;

namespace ProductPhotos.API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly string _uploadsRoot;

        public PhotoService(IWebHostEnvironment environment)
        {
            var webRoot = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
            _uploadsRoot = Path.Combine(webRoot, "uploads", "products");
        }

        public string? Validate(IReadOnlyCollection<IFormFile> files)
        {
            if (files.Count > PhotoRules.MaxFilesPerRequest)
            {
                return $"Можна завантажити не більше {PhotoRules.MaxFilesPerRequest} файлів за один запит";
            }

            foreach (var file in files)
            {
                var name = Path.GetFileName(file.FileName);

                if (file.Length == 0)
                {
                    return $"Файл '{name}' порожній";
                }

                if (file.Length > PhotoRules.MaxFileSize)
                {
                    return $"Файл '{name}' перевищує {PhotoRules.MaxFileSize / (1024 * 1024)} МБ";
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!PhotoRules.AllowedExtensions.Contains(extension))
                {
                    return $"Файл '{name}' має недопустимий формат. Дозволено: {string.Join(", ", PhotoRules.AllowedExtensions)}";
                }

                if (!HasImageSignature(file))
                {
                    return $"Файл '{name}' не є коректним зображенням";
                }
            }

            return null;
        }

        public async Task<List<ProductPhoto>> SaveAsync(int productId, IReadOnlyCollection<IFormFile> files)
        {
            var folder = GetProductFolder(productId);
            Directory.CreateDirectory(folder);

            var photos = new List<ProductPhoto>();

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var storedName = $"{Guid.NewGuid():N}{extension}";
                var path = Path.Combine(folder, storedName);

                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var originalName = Path.GetFileName(file.FileName);

                if (originalName.Length > 255)
                {
                    originalName = originalName[^255..];
                }

                photos.Add(new ProductPhoto
                {
                    ProductId = productId,
                    StoredFileName = storedName,
                    OriginalFileName = originalName,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    CreatedAt = DateTime.UtcNow
                });
            }

            return photos;
        }

        public void DeleteFile(ProductPhoto photo)
        {
            var path = Path.Combine(GetProductFolder(photo.ProductId), photo.StoredFileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeleteProductFolder(int productId)
        {
            var folder = GetProductFolder(productId);

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        private string GetProductFolder(int productId)
        {
            return Path.Combine(_uploadsRoot, productId.ToString());
        }

        private static bool HasImageSignature(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var header = new byte[12];
            var read = stream.Read(header, 0, header.Length);

            if (read < 4)
            {
                return false;
            }

            var isJpeg = header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
            var isPng = header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;
            var isGif = header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x38;
            var isWebp = read >= 12
                && header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46
                && header[8] == 0x57 && header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50;

            return isJpeg || isPng || isGif || isWebp;
        }
    }
}
