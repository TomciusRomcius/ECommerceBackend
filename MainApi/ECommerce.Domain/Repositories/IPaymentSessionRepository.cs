using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IPaymentSessionRepository
{
    public Task CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity);
    public Task<PaymentSessionEntity?> GetPaymentSession(Guid userId);
    public Task DeletePaymentSession(Guid userId);
}