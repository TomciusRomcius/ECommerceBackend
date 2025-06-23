using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface IPaymentSessionRepository
{
    public Task<ResultError?> CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity);
    public Task<PaymentSessionEntity?> GetPaymentSession(Guid userId);
    public Task DeletePaymentSession(Guid userId);
}