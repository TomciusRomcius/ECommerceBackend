namespace ECommerce.Domain.Entities.PaymentSession
{
    public class PaymentSessionEntity
    {
        public string PaymentSessionId { get; set; }
        public Guid UserId { get; set; }
        // TODO: get rid of magical strings
        public string PaymentSessionProvider { get; set; }

        public PaymentSessionEntity(string paymentSessionId, Guid userId, string paymentSessionProvider)
        {
            PaymentSessionId = paymentSessionId;
            UserId = userId;
            PaymentSessionProvider = paymentSessionProvider;
        }
    }
}