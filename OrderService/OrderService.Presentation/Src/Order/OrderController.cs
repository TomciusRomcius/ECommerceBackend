using ECommerceBackend.Utils.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.Entities;

namespace OrderService.Order;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly Application.Services.OrderService _orderService;
    
    public OrderController(Application.Services.OrderService orderService)
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
