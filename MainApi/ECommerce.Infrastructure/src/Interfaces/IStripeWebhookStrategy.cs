using ECommerce.Domain.Utils;
using Stripe;

namespace ECommerce.Infrastructure.src.Interfaces
{
    public interface IStripeWebhookStrategy
    {
        Task<ResultError?> RunAsync(IHasObject ev);
    }
}