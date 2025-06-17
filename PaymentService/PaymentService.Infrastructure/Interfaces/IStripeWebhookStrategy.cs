using PaymentService.Domain.Utils;
using Stripe;

namespace PaymentService.Infrastructure.Interfaces
{
    public interface IStripeWebhookStrategy
    {
        string EventType { get; }
        Task<ResultError?> RunAsync(IHasObject ev);
    }
}