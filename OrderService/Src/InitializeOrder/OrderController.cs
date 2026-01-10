using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

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
        string? userJwt = await HttpContext.GetTokenAsync("access_token");
        if (String.IsNullOrWhiteSpace(userJwt))
            return Forbid();
        if (userId is null) return new UnauthorizedObjectResult("You must be logged in to add items to cart!");

        // TODO: result pattern
        Result<PaymentSessionModel> result = await _orderService.CreateOrderPaymentSession(new Guid(userId), PaymentProvider.STRIPE);
        if (result.Errors.Any())
        {
            return ControllerUtils.ResultErrorsToResponse(result.Errors);
        }
        return Ok(result.GetValue());
    }
}