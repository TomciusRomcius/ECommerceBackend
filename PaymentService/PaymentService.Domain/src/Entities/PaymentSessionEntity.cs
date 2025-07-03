namespace PaymentService.Domain.src.Entities
{
    public class PaymentSessionEntity
    {

        public required string PaymentSessionId { get; set; }

        public required Guid UserId { get; set; }

        // TODO: get rid of magical strings
        public required string PaymentSessionProvider { get; set; }
    }
}
