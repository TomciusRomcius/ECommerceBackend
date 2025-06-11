using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Utils;
using Stripe;

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

    // TODO: not ideal, figure out a way to do this without having to mass a generic parameter
    // everytime this needs to be called
    // T = IHasResult
    public Task<Result<T>> ParseWebhookEvent<T>(string json, string signature)
    {
        if (typeof(T) != typeof(Event))
        {
            return Task.FromResult(new Result<T>([
                new ResultError(ResultErrorType.INVALID_OPERATION_ERROR, "T must be IHasObject")
            ]));
        }
        // TODO: throwOnApiVersionMismatch = true on production
        // Construct webhook event + verify the signature
        Event ev = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSignature,
            throwOnApiVersionMismatch: false);
        return Task.FromResult(new Result<T>((T)(object)ev));
    }
}