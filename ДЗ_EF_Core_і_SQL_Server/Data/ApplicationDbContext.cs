using ClientManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients => Set<Client>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    FullName = "Коваленко Олена Петрівна",
                    Email = "olena.kovalenko@example.com",
                    Phone = "+380501112233",
                    RegistrationDate = new DateTime(2026, 8, 12)
                },
                new Client
                {
                    Id = 2,
                    FullName = "Шевчук Андрій Іванович",
                    Email = "andrii.shevchuk@example.com",
                    Phone = "+380671234567",
                    RegistrationDate = new DateTime(2026, 9, 3)
                },
                new Client
                {
                    Id = 3,
                    FullName = "Мельник Марія Олексіївна",
                    Email = "maria.melnyk@example.com",
                    Phone = "+380931112244",
                    RegistrationDate = new DateTime(2026, 10, 1)
                }
            );
        }
    }
}
