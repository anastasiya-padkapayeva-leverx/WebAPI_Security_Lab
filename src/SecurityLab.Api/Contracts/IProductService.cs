using SecurityLab.Api.Models;

namespace SecurityLab.Api.Contracts;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Product>> SearchAsync(string name, string sortBy, CancellationToken ct = default);
}
