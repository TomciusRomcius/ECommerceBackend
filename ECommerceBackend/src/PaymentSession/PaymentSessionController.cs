using ECommerce.PaymentSession;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerce.Order
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentSessionController : ControllerBase
    {
        readonly IStripeSessionService _stripeSessionService;
        readonly IOrderService _orderService;
        readonly ILogger _logger;

        public PaymentSessionController(IStripeSessionService stripeSessionService, IOrderService orderService, ILogger logger)
        {
            _stripeSessionService = stripeSessionService;
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = _stripeSessionService.ParseWebhookEvent(json);

            if (stripeEvent.Type == EventTypes.ChargeSucceeded)
            {
                var charge = stripeEvent.Data.Object as Charge;
                string? userId = charge?.Metadata["userId"];
                if (userId is not null)
                {
                    await _orderService.OnCharge(new Guid(userId));
                }

                else
                {
                    _logger.LogCritical("Payment intent metadata:userId is null!");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return Ok();
        }
    }
}