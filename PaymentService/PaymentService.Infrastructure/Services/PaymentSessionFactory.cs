using Microsoft.Extensions.Options;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enums;
using PaymentService.Infrastructure.Utils;

namespace PaymentService.Infrastructure.Services;

public class PaymentSessionFactory : IPaymentSessionFactory
{
    private readonly StripeSettings _stripeSettings;

    public PaymentSessionFactory(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }

    public IPaymentSessionService CreatePaymentSessionService(PaymentProvider provider)
    {
        IPaymentSessionService? result = null;

        if (provider == PaymentProvider.STRIPE) result = new StripeSessionService(_stripeSettings);

        if (result is null) throw new InvalidOperationException("Invalid payment provider");

        return result;
    }
}