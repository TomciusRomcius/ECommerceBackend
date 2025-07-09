using ECommerce.Application.src.Interfaces;
using ECommerce.Domain.src.Enums;
using ECommerce.Domain.src.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Presentation.src.Controllers.Order;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("session")]
    public async Task<IActionResult> GetOrderPaymentSession()
    {
        // Fetch session id from db
        return Ok();
    }

    [HttpPost("session")]
    public async Task<IActionResult> CreateOrderPaymentSession(
        [FromQuery(Name = "testcharge")] bool testCharge
    )
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return new UnauthorizedObjectResult("You must be logged in to add items to cart!");

        // TODO: result pattern
        PaymentSessionModel? result = await _orderService.CreateOrderPaymentSession(new Guid(userId), PaymentProvider.STRIPE);
        return Ok(result);
    }
}