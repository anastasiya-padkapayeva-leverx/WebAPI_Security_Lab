using SecurityLab.Api.Dtos;

namespace SecurityLab.Api.Contracts;

public interface IOrderService
{
    Task<IReadOnlyList<OrderResponse>> GetUserOrdersAsync(int userId, CancellationToken ct = default);
    Task<OrderResponse?> GetByIdAsync(int orderId, CancellationToken ct = default);
    Task CancelAsync(int orderId, CancellationToken ct = default);
    Task<OrderResponse> CreateAsync(int userId, CreateOrderRequest request, CancellationToken ct = default);
}
