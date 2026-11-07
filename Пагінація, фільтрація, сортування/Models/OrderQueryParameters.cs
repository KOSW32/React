using System.ComponentModel.DataAnnotations;

namespace OrdersPaging.API.Models
{
    public class OrderQueryParameters
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page має бути не менше 1")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize має бути від 1 до 100")]
        public int PageSize { get; set; } = 10;

        public string? Number { get; set; }

        public decimal? MinTotal { get; set; }

        public decimal? MaxTotal { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public string SortBy { get; set; } = "id";

        public string SortDir { get; set; } = "asc";
    }
}
