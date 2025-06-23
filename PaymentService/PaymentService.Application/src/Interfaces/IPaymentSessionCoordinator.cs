using PaymentService.Domain.src.Entities;
using PaymentService.Domain.src.Enums;
using PaymentService.Domain.src.Utils;

namespace PaymentService.Application.src.Interfaces
{
    public interface IPaymentSessionCoordinator
    {
        Task<Result<PaymentSessionEntity?>> CreatePaymentSessionAsync(PaymentProvider paymentProvider, GeneratePaymentSessionOptions options);
        Task<PaymentSessionEntity?> GetUserSessionAsync(Guid userId);
    }
}