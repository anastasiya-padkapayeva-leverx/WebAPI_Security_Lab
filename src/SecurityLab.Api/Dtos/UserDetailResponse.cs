namespace SecurityLab.Api.Dtos;

public class UserDetailResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int TokenVersion { get; set; }
}
