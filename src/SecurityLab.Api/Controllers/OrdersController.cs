using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLab.Api.Common;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Dtos;

namespace SecurityLab.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService) => _orderService = orderService;

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetMyOrders(CancellationToken ct)
    {
        return Ok(await _orderService.GetUserOrdersAsync(CurrentUserId, ct));
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(int orderId, CancellationToken ct)
    {
        var order = await _orderService.GetByIdAsync(orderId, ct);
        if (order is null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost("{orderId:int}/cancel")]
    public async Task<IActionResult> CancelOrder(int orderId, CancellationToken ct)
    {
        try
        {
            await _orderService.CancelAsync(orderId, ct);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request, CancellationToken ct)
    {
        return Ok(await _orderService.CreateAsync(CurrentUserId, request, ct));
    }
}
