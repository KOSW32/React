using UserManager.Models;

namespace UserManager.Data
{
    public class UserStore
    {
        private static readonly string[] LastNames = { "Коваленко", "Шевченко", "Бондаренко", "Ткаченко", "Мельник", "Кравченко" };
        private static readonly string[] FirstNames = { "Олена", "Андрій", "Марія", "Ігор" };

        public List<User> Users { get; } = Enumerable.Range(1, 24)
            .Select(i => new User
            {
                Id = i,
                FullName = $"{LastNames[i % LastNames.Length]} {FirstNames[i % FirstNames.Length]}",
                Email = $"user{i}@example.com",
                Phone = $"+380{50 + i % 40}{1000000 + i * 13579:D7}",
                BirthDate = new DateTime(1985, 1, 1).AddDays(i * 400),
                Role = (UserRole)(i % 3),
                IsActive = i % 5 != 0,
                CreatedAt = new DateTime(2026, 6, 1, 10, 0, 0).AddDays(i * 3)
            })
            .ToList();

        public bool EmailExists(string email, int? excludeId = null)
        {
            var normalized = email.Trim();

            return Users.Any(u =>
                u.Email.Equals(normalized, StringComparison.OrdinalIgnoreCase) && u.Id != excludeId);
        }
    }
}
