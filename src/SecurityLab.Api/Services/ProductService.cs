using Microsoft.EntityFrameworkCore;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Data;
using SecurityLab.Api.Models;

namespace SecurityLab.Api.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Products.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string name, string sortBy, CancellationToken ct = default)
    {
        var sql = $"SELECT * FROM Products WHERE Name LIKE '%{name}%' ORDER BY {sortBy}";

        return await _db.Products.FromSqlRaw(sql).ToListAsync(ct);
    }
}
