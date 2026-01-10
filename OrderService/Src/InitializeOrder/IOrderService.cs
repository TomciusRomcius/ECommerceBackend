using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public interface IOrderService
{
    public Task<Result<PaymentSessionModel>> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider);
}