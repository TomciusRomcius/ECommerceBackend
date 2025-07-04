using Microsoft.Extensions.Options;
using PaymentService.Application.src.Interfaces;
using PaymentService.Domain.src.Enums;
using PaymentService.Infrastructure.src.Utils;

namespace PaymentService.Infrastructure.src.Services;

public class PaymentSessionFactory : IPaymentSessionFactory
{
    private readonly StripeSettings _stripeSettings;

    public PaymentSessionFactory(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }

    public IProviderPaymentSessionService CreatePaymentSessionService(PaymentProvider provider)
    {
        IProviderPaymentSessionService? result = null;

        if (provider == PaymentProvider.STRIPE) result = new StripeSessionService(_stripeSettings);

        if (result is null) throw new InvalidOperationException("Invalid payment provider");

        return result;
    }
}