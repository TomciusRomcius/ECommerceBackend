using ECommerce.Application.UseCases.PaymentSession.Notifications;
using ECommerce.Domain.Enums.PaymentProvider;
using ECommerce.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Order
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentSessionController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IPaymentSessionFactory _paymentSessionFactory;

        public PaymentSessionController(IMediator mediator, IPaymentSessionFactory paymentSessionFactory)
        {
            _mediator = mediator;
            _paymentSessionFactory = paymentSessionFactory;
        }

        // [HttpPost("webhook")]
        // public async Task<IActionResult> StripeWebhook()
        // {
        //     string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //     string? signature = Request.Headers["Stripe-Signature"];

        //     var stripeEvent = EventUtility.ConstructEvent(
        //         json,
        //         signature,
        //         _stripeSettings.WebhookSignature,
        //         throwOnApiVersionMismatch: false
        //     );

        //     if (stripeEvent.Type == EventTypes.ChargeSucceeded)
        //     {
        //         var charge = stripeEvent.Data.Object as Charge;
        //         string? userId = charge?.Metadata["userid"];
        //         if (userId is not null)
        //         {
        //             await _orderService.OnCharge(new Guid(userId));
        //         }

        //         else
        //         {
        //             _logger.LogCritical("Payment intent metadata:userId is null!");
        //             return StatusCode(StatusCodes.Status500InternalServerError);
        //         }
        //     }
        //     return Ok();
        // }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string? signature = Request.Headers["Stripe-Signature"];

            var paymentSessionService = _paymentSessionFactory.CreatePaymentSessionService(PaymentProvider.STRIPE);

            await _mediator.Publish(new WebhookNotification(json, signature, paymentSessionService));

            return Ok();
        }
    }
}