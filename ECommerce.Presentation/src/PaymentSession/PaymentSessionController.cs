using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.PaymentSession;

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
        if (String.IsNullOrWhiteSpace(json) || String.IsNullOrWhiteSpace(signature))
        {
            return Forbid();
        }
        await _webhookCoordinatorService.HandlePaymentWebhook(json, signature);
        return Ok();
    }
}