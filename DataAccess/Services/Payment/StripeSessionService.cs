using ECommerce.DataAccess.Utils;
using Microsoft.Extensions.Logging;
using Stripe;

namespace ECommerce.PaymentSession
{
    public class StripeSessionService : IStripeSessionService
    {
        readonly StripeSettings _stripeSettings;
        readonly ILogger _logger;

        public StripeSessionService(StripeSettings stripeSettings, ILogger logger)
        {
            _stripeSettings = stripeSettings;
            _logger = logger;
            StripeConfiguration.ApiKey = _stripeSettings.ApiKey;
        }

        public PaymentIntent GeneratePaymentSession(string userId)
        {

            var options = new PaymentIntentCreateOptions
            {
                Amount = 10 * 100,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "userId", userId }
                }
            };

            var service = new PaymentIntentService();

            return service.Create(options);
        }

        public void HandleWebhook(string json)
        {
            var stripeEvent = EventUtility.ParseEvent(json, false);

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var intent = stripeEvent.Data.Object as PaymentIntent;
                string? userId = intent.Metadata["userId"];
                _logger.LogInformation("Payment succeeded");
                _logger.LogInformation("Uid {UserId}", userId);
            }

            else _logger.LogInformation(stripeEvent.Type);
        }
    }
}