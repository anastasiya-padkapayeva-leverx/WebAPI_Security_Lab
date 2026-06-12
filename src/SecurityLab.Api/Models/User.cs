using SecurityLab.Api.Common;

namespace SecurityLab.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.User;
    public int TokenVersion { get; set; } = 1;
    public int FailedLoginAttempts { get; set; }
    public DateTimeOffset? LockoutEndUtc { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
