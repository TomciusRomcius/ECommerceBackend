using Stripe;

namespace ECommerce.PaymentSession
{
    public interface IStripeSessionService
    {
        public PaymentIntent GeneratePaymentSession(string userId);
        public void HandleWebhook(string json);
    }
}