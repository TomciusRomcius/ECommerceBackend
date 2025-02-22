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

            return result;
        }

        public PaymentIntent GeneratePaymentSessionAndConfirm(GeneratePaymentSessionOptions sessionOptions)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = sessionOptions.Price,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "userid", sessionOptions.UserId }
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

        public Event ParseWebhookEvent(string json)
        {
            return EventUtility.ParseEvent(json, false);
        }
    }
}