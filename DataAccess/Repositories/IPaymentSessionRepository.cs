using ECommerce.DataAccess.Entities.PaymentSession;

namespace ECommerce.DataAccess.Repositories.PaymentSession
{
    public interface IPaymentSessionRepository
    {
        public Task CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity);
        public Task<PaymentSessionEntity?> GetPaymentSession(Guid userId);
        public Task DeletePaymentSession(Guid userId);
    }
}