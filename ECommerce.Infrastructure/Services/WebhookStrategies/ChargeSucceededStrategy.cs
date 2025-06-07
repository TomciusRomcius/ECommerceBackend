using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Interfaces;
using Stripe;
using System.Data;

namespace ECommerce.Application.Services.WebhookStrategies
{
    public class ChargeSucceededStrategy : IStripeWebhookStrategy
    {
        public ResultError? Run(IHasObject ev)
        {
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

            if (charge is null) throw new DataException("Failed parsing charge");

            string? userId;
            charge.Metadata.TryGetValue("userid", out userId);

            return null;
        }
    }
}
