using System.Data;
using ECommerce.DataAccess.Utils;
using ECommerce.Domain.Enums.PaymentProvider;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models.PaymentSession;
using Stripe;

namespace ECommerce.PaymentSession
{
    public class StripeSessionService : IPaymentSessionService
    {
        readonly StripeSettings _stripeSettings;

        public StripeSessionService(StripeSettings stripeSettings)
        {
            _stripeSettings = stripeSettings;
            StripeConfiguration.ApiKey = _stripeSettings.ApiKey;
        }

        public async Task<PaymentProviderSession> GeneratePaymentSession(GeneratePaymentSessionOptions sessionOptions)
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
            var result = await service.CreateAsync(options);
            // TODO: have not hard coded currency
            return new PaymentProviderSession
            {
                Provider = PaymentProvider.STRIPE,
                UserId = sessionOptions.UserId,
                ClientSecret = result.ClientSecret,
                SessionId = result.Id,
                Currency = "usd",
            };
        }

        public Task<PaymentProviderEvent> ParseWebhookEvent(string json, string signature)
        {
            // TODO: throwOnApiVersionMismatch = true on production
            Event ev = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSignature, throwOnApiVersionMismatch: false);
            PaymentProviderEvent? result = null;

            if (ev.Type == EventTypes.ChargeSucceeded)
            {
                Charge? charge = ev.Data.Object as Charge;
                if (charge is null)
                {
                    throw new DataException("Failed parsing charge");
                }

                if (!charge.Metadata.ContainsKey("userid"))
                {
                    throw new DataException("userid is null!");
                }

                // TODO: null safety
                result = new PaymentProviderEvent
                {
                    UserId = charge.Metadata["userid"],
                    Id = charge.Id,
                    Provider = PaymentProvider.STRIPE,
                    EventType = PaymentProviderEventType.CHARGE_SUCEEDED
                };
            }

            if (result is null)
            {
                // TODO: log warning
            }

            return Task.FromResult(result);
        }
    }
}