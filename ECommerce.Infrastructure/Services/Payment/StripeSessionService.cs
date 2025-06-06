using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Utils;
using Stripe;
using System.Data;

namespace ECommerce.Infrastructure.Services.Payment;

public class StripeSessionService : IPaymentSessionService
{
    private readonly StripeSettings _stripeSettings;

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
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
                AllowRedirects = "never"
            }
        };

        var service = new PaymentIntentService();
        PaymentIntent? result = await service.CreateAsync(options);
        // TODO: have not hard coded currency
        return new PaymentProviderSession
        {
            Provider = PaymentProvider.STRIPE,
            UserId = sessionOptions.UserId,
            ClientSecret = result.ClientSecret,
            SessionId = result.Id,
            Currency = "usd"
        };
    }

    public Task<Result<PaymentProviderEvent>> ParseWebhookEvent(string json, string signature)
    {
        // TODO: throwOnApiVersionMismatch = true on production
        // TODO: refactor event types into a Dictionary to use strategy pattern
        // Construct webhook event + verify the signature
        Event ev = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSignature,
            throwOnApiVersionMismatch: false);
        Result<PaymentProviderEvent> result;

        switch (ev.Type)
        {
            case EventTypes.ChargeSucceeded:
                {
                    var charge = ev.Data.Object as Charge;
                    if (charge is null) throw new DataException("Failed parsing charge");

                    string? userId;
                    if (!charge.Metadata.TryGetValue("userid", out userId))
                    {
                        var error = new ResultError(
                            ResultErrorType.VALIDATION_ERROR,
                            "Event does not have a user id attached to it"
                        );

                        result = new Result<PaymentProviderEvent>([error]);
                        break;
                    }

                    result = new Result<PaymentProviderEvent>(new PaymentProviderEvent
                    {
                        UserId = userId,
                        Id = charge.Id,
                        Provider = PaymentProvider.STRIPE,
                        EventType = PaymentProviderEventType.CHARGE_SUCEEDED
                    });
                    break;
                }
            default:
                result = new Result<PaymentProviderEvent>([
                    new ResultError(ResultErrorType.UNSUPPORTED, "Payment event type unsupported")
                ]);
                break;
        }

        return Task.FromResult(result);
    }
}