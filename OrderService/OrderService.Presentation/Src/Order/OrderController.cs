using ECommerceBackend.Utils.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Services;
using OrderService.Domain.Entities;

namespace OrderService.Presentation.Order;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(Roles = RoleTypes.Admin)]
    [HttpGet]
    public async Task<ActionResult<OrderEntity?>> GetOrder([FromQuery] Guid userId, [FromQuery] Guid orderId)
    {
        OrderEntity? order = await _orderService.GetOrderAsync(userId, orderId);
        return Ok(order);
    }
}
