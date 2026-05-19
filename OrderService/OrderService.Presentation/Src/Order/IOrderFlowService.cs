using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.Presentation.Order;

public interface IOrderFlowService
{
    public Task<Result<PaymentSessionModel>> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider);
}
