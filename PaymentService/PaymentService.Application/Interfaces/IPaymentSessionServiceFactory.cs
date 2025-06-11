using PaymentService.Domain.Enums;

namespace PaymentService.Application.Interfaces;

public interface IPaymentSessionFactory
{
    IPaymentSessionService CreatePaymentSessionService(PaymentProvider provider);
}