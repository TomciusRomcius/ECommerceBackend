using ECommerce.Domain.src.Enums;

namespace ECommerce.Domain.src.Models.PaymentSession;

public class PaymentProviderEvent
{
    public required PaymentProvider Provider { get; set; }
    public required string EventType { get; set; }
    public required Dictionary<string, object> Data { get; set; }
}