using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.PaymentSession;

namespace ECommerce.Application.Interfaces;

public interface IOrderService
{
    public Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider);
    public Task OnCharge(Guid userId);
}