using ECommerce.Domain.src.Enums;

namespace ECommerce.Domain.src.Models
{
    public class PaymentSessionModel
    {
        public required string PaymentSessionId { get; set; }
        public required string ClientSecret { get; set; }
        public required Guid UserId { get; set; }
        public required PaymentProvider PaymentSessionProvider { get; set; }
    }
}
