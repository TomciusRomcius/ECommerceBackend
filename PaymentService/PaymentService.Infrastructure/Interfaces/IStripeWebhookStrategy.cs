using PaymentService.Domain.Utils;
using Stripe;

namespace PaymentService.Infrastructure.Interfaces
{
    public interface IStripeWebhookStrategy
    {
        Task<ResultError?> RunAsync(IHasObject ev);
    }
}