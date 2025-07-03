using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Models;

public class PaymentProviderSession
{
    public required string UserId { get; set; } // Retrieved from metadata
    public required string SessionId { get; set; }
    public required string ClientSecret { get; set; }
    public required string Currency { get; set; }
    public required PaymentProvider Provider { get; set; }
}