using ECommerce.Domain.src.Enums;
using ECommerce.Domain.src.Models;

namespace ECommerce.Application.src.Interfaces;

public interface IOrderService
{
    // TODO: result pattern
    public Task<PaymentSessionModel?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider);
}