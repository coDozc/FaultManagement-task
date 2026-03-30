using FaultManagement.Domain.Enums;

namespace FaultManagement.Domain.Entities;

public class AppUser
{
    public Guid Id { get; init; }
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private AppUser()
    {
    }

    public AppUser(string userName, string email, string passwordHash, UserRole role)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAtUtc = DateTime.UtcNow;
    }

    // Constructor for seeding with specific ID
    public AppUser(Guid id, string userName, string email, string passwordHash, UserRole role)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
