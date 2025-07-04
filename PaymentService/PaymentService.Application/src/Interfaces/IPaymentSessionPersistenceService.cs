using PaymentService.Domain.src.Enums;
using PaymentService.Domain.src.Utils;

namespace PaymentService.Application.src.Interfaces
{
    public interface IPaymentSessionService
    {
        Task<ResultError?> CreateAsync(GeneratePaymentSessionOptions options, PaymentProvider provider);
    }
}
