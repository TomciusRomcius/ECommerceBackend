using Stripe;

namespace ECommerce.PaymentSession
{
    public interface IStripeSessionService
    {
        public PaymentIntent GeneratePaymentSession(GeneratePaymentSessionOptions sessionOptions);
        public void HandleWebhook(string json);
        /// <summary>
        /// Used for testing Stripe webhooks. Instantly confirms payment using a testing Stripe card.
        /// </summary>
        public void GeneratePaymentSessionAndConfirm(GeneratePaymentSessionOptions sessionOptions);
    }
}