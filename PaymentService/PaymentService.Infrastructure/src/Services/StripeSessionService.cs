using PaymentService.Application.src.Interfaces;
using PaymentService.Domain.src.Enums;
using PaymentService.Domain.src.Models;
using PaymentService.Domain.src.Utils;
using PaymentService.Infrastructure.src.Utils;
using Stripe;

namespace PaymentService.Infrastructure.src.Services;

public class StripeSessionService : IProviderPaymentSessionService
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
                { "userId", sessionOptions.UserId.ToString() },
                { "orderId", sessionOptions.OrderId.ToString() },
            },
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
                AllowRedirects = "never"
            }
        };

        var service = new PaymentIntentService();
        PaymentIntent? result = await service.CreateAsync(options);
        return new PaymentProviderSession
        {
            Provider = PaymentProvider.STRIPE,
            UserId = sessionOptions.UserId.ToString(),
            ClientSecret = result.ClientSecret,
            SessionId = result.Id,
            Currency = "usd"
        };
    }

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
        Event ev = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSecret,
            throwOnApiVersionMismatch: false);
        return Task.FromResult(new Result<T>((T)(object)ev));
    }
}