using SecurityLab.Api.Models;

namespace SecurityLab.Api.Contracts;

public interface ITokenService
{
    string GenerateToken(User user);
}
