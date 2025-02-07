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

        public PaymentIntent GeneratePaymentSession(GeneratePaymentSessionOptions sessionOptions)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = sessionOptions.Price,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "userId", sessionOptions.UserId }
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                {
                    Enabled = true,
                    AllowRedirects = "never"
                }
            };

            var service = new PaymentIntentService();
            var result = service.Create(options);

            service.Confirm(result.Id, new()
            {
                PaymentMethod = "pm_card_visa"
            });

            return result;
        }

        public void GeneratePaymentSessionAndConfirm(GeneratePaymentSessionOptions sessionOptions)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = sessionOptions.Price,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "userId", sessionOptions.UserId }
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                {
                    Enabled = true,
                    AllowRedirects = "never"
                }
            };

            var service = new PaymentIntentService();
            var result = service.Create(options);

            service.Confirm(result.Id, new()
            {
                PaymentMethod = "pm_card_visa"
            });
        }

        public void HandleWebhook(string json)
        {
            var stripeEvent = EventUtility.ParseEvent(json, false);

            if (stripeEvent.Type == EventTypes.ChargeSucceeded)
            {
                var charge = stripeEvent.Data.Object as Charge;
                string? userId = charge?.Metadata["userId"];
                _logger.LogInformation("Payment succeeded");
                _logger.LogInformation(userId);
            }
        }
    }
}