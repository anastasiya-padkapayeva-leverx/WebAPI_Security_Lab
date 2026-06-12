using Microsoft.EntityFrameworkCore;
using SecurityLab.Api.Common;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Data;
using SecurityLab.Api.Dtos;
using SecurityLab.Api.Models;

namespace SecurityLab.Api.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<OrderResponse>> GetUserOrdersAsync(int userId, CancellationToken ct = default)
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync(ct);

        return orders.Select(ToResponse).ToList();
    }

    public async Task<OrderResponse?> GetByIdAsync(int orderId, CancellationToken ct = default)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        return order is null ? null : ToResponse(order);
    }

    public async Task CancelAsync(int orderId, CancellationToken ct = default)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId, ct)
            ?? throw new NotFoundException($"Order {orderId} not found.");

        order.Status = OrderStatus.Cancelled;
        await _db.SaveChangesAsync(ct);
    }

    public async Task<OrderResponse> CreateAsync(int userId, CreateOrderRequest request, CancellationToken ct = default)
    {
        var order = new Order
        {
            UserId    = userId,
            Status    = OrderStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            Items     = request.Items
                .Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity    = i.Quantity,
                    UnitPrice   = i.UnitPrice
                })
                .ToList()
        };
        order.Total = order.Items.Sum(i => i.Quantity * i.UnitPrice);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);

        return ToResponse(order);
    }

    private static OrderResponse ToResponse(Order o) => new()
    {
        Id = o.Id,
        UserId = o.UserId,
        Status = o.Status.ToString(),
        Total = o.Total,
        CreatedAt = o.CreatedAt,
        Items = o.Items.Select(i => new OrderItemResponse
        {
            Id = i.Id,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };
}
