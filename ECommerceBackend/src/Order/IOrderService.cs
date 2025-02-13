using Stripe;

namespace ECommerce.Order
{
    public interface IOrderService
    {
        public Task<PaymentIntent?> CreateOrderPaymentSession(Guid userId);
        public Task OnCharge(Guid userId);
    }
}