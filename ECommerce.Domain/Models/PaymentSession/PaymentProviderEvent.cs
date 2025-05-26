using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models.PaymentSession;

public class PaymentProviderEvent
{
    public required PaymentProviderEventType EventType { get; set; }
    public required string UserId { get; set; } // Will be provided in the metadata
    public required string Id { get; set; }
    public required PaymentProvider Provider { get; set; }
}