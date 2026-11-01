using Microsoft.EntityFrameworkCore;
using TaskTracker.API.Models;
using TaskStatus = TaskTracker.API.Models.TaskStatus;

namespace TaskTracker.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var demoProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(1000);
                entity.HasQueryFilter(p => !p.IsDeleted);

                entity.HasData(new Project
                {
                    Id = demoProjectId,
                    Name = "Навчальний проєкт",
                    Description = "Демонстраційний проєкт із тестовими завданнями",
                    CreatedAt = new DateTime(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                });
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(150);
                entity.Property(t => t.Description).HasMaxLength(2000);
                entity.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);
                entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
                entity.HasQueryFilter(t => !t.IsDeleted);

                entity.HasOne(t => t.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasData(
                    new TaskItem
                    {
                        Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
                        Title = "Налаштувати базу даних",
                        Description = "Створити міграцію та застосувати її до LocalDB",
                        Priority = TaskPriority.High,
                        Status = TaskStatus.Done,
                        ProjectId = demoProjectId,
                        CreatedAt = new DateTime(2026, 10, 1, 9, 30, 0, DateTimeKind.Utc),
                        IsDeleted = false
                    },
                    new TaskItem
                    {
                        Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"),
                        Title = "Реалізувати контролери",
                        Description = "Projects і Tasks з сервісним шаром",
                        Priority = TaskPriority.Medium,
                        Status = TaskStatus.InProgress,
                        ProjectId = demoProjectId,
                        CreatedAt = new DateTime(2026, 10, 2, 10, 0, 0, DateTimeKind.Utc),
                        IsDeleted = false
                    });
            });
        }
    }
}
