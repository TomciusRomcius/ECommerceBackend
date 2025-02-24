namespace ECommerce.Domain.Models.PaymentSession
{
    public enum PaymentProviderEventType
    {
        CHARGE_SUCEEDED,
        CHARGE_EXPIRED,
        CHARGE_REFUNDED,
        CHARGE_PENDING
    }
}