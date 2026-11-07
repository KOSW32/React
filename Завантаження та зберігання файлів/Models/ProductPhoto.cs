namespace ProductPhotos.API.Models
{
    public class ProductPhoto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string StoredFileName { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long Size { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
