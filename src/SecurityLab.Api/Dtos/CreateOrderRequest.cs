namespace SecurityLab.Api.Dtos;

public class CreateOrderRequest
{
    public IReadOnlyList<CreateOrderItemRequest> Items { get; set; } = new List<CreateOrderItemRequest>();
}
