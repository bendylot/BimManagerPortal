using BimManagerPortal.Domain.Common;

namespace BimManagerPortal.Domain.Entities.Users;

public class User : BaseEntity
{
    protected User() { }

    public User(string googleId, string email, string name, string? avatarUrl) : base()
    {
        GoogleId = googleId;
        Email = email;
        Name = name;
        AvatarUrl = avatarUrl;
        Role = UserRole.Unregistered;
    }

    public string GoogleId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public UserRole Role { get; set; }
    public string? WorkComputer { get; set; }
    public string? EmployeeId { get; set; }
}
