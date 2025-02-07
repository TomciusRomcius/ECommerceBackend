using ECommerce.PaymentSession;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Order
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentSessionController : ControllerBase
    {
        IStripeSessionService _stripeSessionService;

        public PaymentSessionController(IStripeSessionService stripeSessionService)
        {
            _stripeSessionService = stripeSessionService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetPaymentSession()
        // {
        //     string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     if (userId is null)
        //     {
        //         return new UnauthorizedObjectResult("You must be logged in to create payment session!");
        //     }

        //     var res = _stripeSessionService.GeneratePaymentSession(userId);
        //     return Ok(res);
        // }

        // [HttpPost]
        // public async Task<IActionResult> CreatePaymentSession()
        // {
        //     string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     if (userId is null)
        //     {
        //         return new UnauthorizedObjectResult("You must be logged in to create payment session!");
        //     }

        //     var res = _stripeSessionService.GeneratePaymentSession(userId);
        //     return Ok(res);
        // }

        [HttpPost("webhook")]
        public async Task<IActionResult> TestFulfillPayment()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            _stripeSessionService.HandleWebhook(json);
            return Ok();
        }
    }
}