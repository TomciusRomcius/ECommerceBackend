using ECommerce.Application.src.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.src.Controllers.PaymentSession;

[ApiController]
[Route("[controller]")]
public class PaymentSessionController : ControllerBase
{
    private readonly IWebhookCoordinatorService _webhookCoordinatorService;

    public PaymentSessionController(IWebhookCoordinatorService webhookCoordinatorService)
    {
        _webhookCoordinatorService = webhookCoordinatorService;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        string? signature = Request.Headers["Stripe-Signature"];
        if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(signature))
        {
            return BadRequest();
        }
        await _webhookCoordinatorService.HandlePaymentWebhook(json, signature);
        return Ok();
    }
}