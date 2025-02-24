using ECommerce.Domain.Enums.PaymentProvider;

namespace ECommerce.Domain.Interfaces.Services
{
    public interface IPaymentSessionFactory
    {
        IPaymentSessionService CreatePaymentSessionService(PaymentProvider provider);
    }
}