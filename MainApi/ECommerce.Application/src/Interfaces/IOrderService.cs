using ECommerce.Domain.src.Enums;
using ECommerce.Domain.src.Models.PaymentSession;

namespace ECommerce.Application.src.Interfaces;

public interface IOrderService
{
    public Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider);
}