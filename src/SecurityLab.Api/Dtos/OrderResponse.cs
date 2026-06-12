namespace SecurityLab.Api.Dtos;

public class OrderResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IReadOnlyList<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
}
