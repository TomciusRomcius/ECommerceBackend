using ECommerce.Domain.Utils;
using Stripe;

namespace ECommerce.Infrastructure.Interfaces
{
    public interface IStripeWebhookStrategy
    {
        ResultError? Run(IHasObject ev);
    }
}