namespace ProductPhotos.API.DTOs
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PhotoResponseDto> Photos { get; set; } = new();
    }
}
