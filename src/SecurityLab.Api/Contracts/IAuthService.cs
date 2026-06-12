using SecurityLab.Api.Dtos;

namespace SecurityLab.Api.Contracts;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<string> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<string> ChangePasswordAsync(int userId, ChangePasswordRequest request, CancellationToken ct = default);
}
