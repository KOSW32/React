namespace UserManager.Models
{
    public class UserQueryParameters
    {
        public static readonly string[] SortFields = { "id", "fullName", "email", "role", "createdAt" };

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public UserRole? Role { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }

        public string SortBy { get; set; } = "id";

        public string SortDir { get; set; } = "asc";
    }
}
