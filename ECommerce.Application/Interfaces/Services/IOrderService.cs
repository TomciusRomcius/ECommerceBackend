using ECommerce.Domain.Enums.PaymentProvider;
using ECommerce.Domain.Models.PaymentSession;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider);
        public Task OnCharge(Guid userId);
    }
}