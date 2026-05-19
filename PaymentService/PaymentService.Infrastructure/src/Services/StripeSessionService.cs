using PaymentService.Application.src.Interfaces;
using PaymentService.Domain.src.Enums;
using PaymentService.Domain.src.Models;
using PaymentService.Domain.src.Utils;
using PaymentService.Infrastructure.src.Utils;
using Stripe;
using Stripe.Checkout;

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
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = $"{_stripeSettings.CheckoutSuccessUrl}&session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = _stripeSettings.CheckoutCancelUrl,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = sessionOptions.Price,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Order",
                        },
                    },
                },
            ],
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "userid", sessionOptions.UserId.ToString() },
                    { "orderId", sessionOptions.OrderId.ToString() },
                },
            },
        };

        Session session = await new SessionService().CreateAsync(options);

        return new PaymentProviderSession
        {
            Provider = PaymentProvider.STRIPE,
            UserId = sessionOptions.UserId.ToString(),
            SessionId = session.Id,
            CheckoutUrl = session.Url!,
            Currency = "usd",
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

        Event ev = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSecret,
            throwOnApiVersionMismatch: false);
        return Task.FromResult(new Result<T>((T)(object)ev));
    }
}
