using Microsoft.EntityFrameworkCore;
using SecurityLab.Api.Common;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Data;
using SecurityLab.Api.Dtos;

namespace SecurityLab.Api.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _db;

    public AdminService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<UserDetailResponse>> GetUsersAsync(CancellationToken ct = default)
    {
        return await _db.Users
            .Select(u => new UserDetailResponse
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role,
                PasswordHash = u.PasswordHash,
                TokenVersion = u.TokenVersion
            })
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<UserDetailResponse>> SearchUsersAsync(string email, CancellationToken ct = default)
    {
        var sql = $"SELECT * FROM Users WHERE Email LIKE '%{email}%'";

        var users = await _db.Users.FromSqlRaw(sql).ToListAsync(ct);

        return users.Select(u => new UserDetailResponse
        {
            Id           = u.Id,
            Email        = u.Email,
            Role         = u.Role,
            PasswordHash = u.PasswordHash,
            TokenVersion = u.TokenVersion
        }).ToList();
    }

    public async Task DeleteUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
            ?? throw new NotFoundException($"User {id} not found.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
    }
}
