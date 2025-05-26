using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Interfaces.Services;

public interface IPaymentSessionFactory
{
    IPaymentSessionService CreatePaymentSessionService(PaymentProvider provider);
}