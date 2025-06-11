using ECommerce.Application.Interfaces;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Interfaces;
using Stripe;

namespace ECommerce.Application.Services.WebhookStrategies
{
    public class ChargeSucceededStrategy : IStripeWebhookStrategy
    {
        IOrderService _orderService;

        public ChargeSucceededStrategy(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<ResultError?> RunAsync(IHasObject ev)
        {
            if (ev is not Charge)
            {
                return new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Event is not of type Charge!"
                );
            }
            var charge = ev as Charge;

            if (charge is null)
            {
                return new ResultError(ResultErrorType.VALIDATION_ERROR, "Charge object is null!");
            }
            charge.Metadata.TryGetValue("userid", out string? userId);
            if (userId == null)
            {
                return new ResultError(
                    ResultErrorType.VALIDATION_ERROR,
                    "Charge event metadata does not have an user id attatched to it"
                );
            }

            await _orderService.OnCharge(new Guid(userId));

            return null;
        }
    }
}
