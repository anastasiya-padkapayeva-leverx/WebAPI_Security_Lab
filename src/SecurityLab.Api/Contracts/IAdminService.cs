using SecurityLab.Api.Dtos;

namespace SecurityLab.Api.Contracts;

public interface IAdminService
{
    Task<IReadOnlyList<UserDetailResponse>> GetUsersAsync(CancellationToken ct = default);
    Task DeleteUserAsync(int id, CancellationToken ct = default);
}
