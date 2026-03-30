using FaultManagement.Domain.Entities;
using FaultManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FaultManagement.Infrastructure.Data;

public class FaultManagementDbContext : DbContext
{
    public DbSet<AppUser> Users { get; set; } = null!;
    public DbSet<FaultNotification> FaultNotifications { get; set; } = null!;

    public FaultManagementDbContext(DbContextOptions<FaultManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AppUser configuration
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).HasConversion<int>();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.UserName).IsUnique();
        });

        // FaultNotification configuration
        modelBuilder.Entity<FaultNotification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Priority).HasConversion<int>();
            entity.Property(e => e.Status).HasConversion<int>();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.CreatedByUserId);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var adminId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var userId = Guid.Parse("20000000-0000-0000-0000-000000000001");

        var adminUser = new AppUser(
            adminId,
            "admin",
            "admin@example.com",
            BCrypt.Net.BCrypt.HashPassword("admin123"),
            UserRole.Admin);

        var normalUser = new AppUser(
            userId,
            "user",
            "user@example.com",
            BCrypt.Net.BCrypt.HashPassword("user123"),
            UserRole.User);

        modelBuilder.Entity<AppUser>().HasData(adminUser, normalUser);

        // Seed 15 fault notifications with different statuses and priorities
        var faultNotifications = new List<FaultNotification>();
        var locations = new[] { "Istanbul", "Ankara", "Izmir", "Bursa", "Antalya", "Gaziantep", "Konya", "Kayseri" };
        var priorities = new[] { PriorityLevel.Low, PriorityLevel.Medium, PriorityLevel.High };
        var statuses = new[] { FaultStatus.New, FaultStatus.UnderReview, FaultStatus.Assigned, FaultStatus.InProgress, FaultStatus.Completed };

        for (int i = 1; i <= 15; i++)
        {
            var notification = new FaultNotification(
                Guid.Parse($"{1000 + i:00000000}-0000-0000-0000-000000000001"),
                $"Fault #{i}",
                $"Description for fault notification {i}",
                locations[i % locations.Length],
                priorities[(i - 1) % priorities.Length],
                statuses[(i - 1) % statuses.Length],
                userId);

            faultNotifications.Add(notification);
        }

        modelBuilder.Entity<FaultNotification>().HasData(faultNotifications);
    }
}
