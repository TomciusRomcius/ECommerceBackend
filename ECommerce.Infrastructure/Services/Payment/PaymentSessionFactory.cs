using ECommerce.DataAccess.Utils;
using ECommerce.Domain.Enums.PaymentProvider;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.PaymentSession;

namespace ECommerce.Infrastructure.Services.Payment
{
    public class PaymentSessionFactory : IPaymentSessionFactory
    {
        readonly StripeSettings _stripeSettings;

        public PaymentSessionFactory(StripeSettings stripeSettings)
        {
            _stripeSettings = stripeSettings;
        }

        public IPaymentSessionService CreatePaymentSessionService(PaymentProvider provider)
        {
            IPaymentSessionService? result = null;

            if (provider == PaymentProvider.STRIPE)
            {
                result = new StripeSessionService(_stripeSettings);
            }

            if (result is null)
            {
                throw new InvalidOperationException($"Invalid payment provider");
            }

            return result;
        }
    }
}