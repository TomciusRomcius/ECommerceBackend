using ECommerce.Domain.Entities.PaymentSession;

namespace ECommerce.Domain.Repositories.PaymentSession
{
    public interface IPaymentSessionRepository
    {
        public Task CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity);
        public Task<PaymentSessionEntity?> GetPaymentSession(Guid userId);
        public Task DeletePaymentSession(Guid userId);
    }
}