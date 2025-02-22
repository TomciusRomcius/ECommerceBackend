using ECommerce.DataAccess.Utils;
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
        readonly StripeSettings _stripeSettings;
        readonly ILogger _logger;

        public PaymentSessionController(IStripeSessionService stripeSessionService, IOrderService orderService, ILogger logger, StripeSettings stripeSettings)
        {
            _stripeSessionService = stripeSessionService;
            _orderService = orderService;
            _logger = logger;
            _stripeSettings = stripeSettings;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string? signature = Request.Headers["Stripe-Signature"];

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signature,
                _stripeSettings.WebhookSignature,
                throwOnApiVersionMismatch: false
            );

            if (stripeEvent.Type == EventTypes.ChargeSucceeded)
            {
                var charge = stripeEvent.Data.Object as Charge;
                string? userId = charge?.Metadata["userid"];
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