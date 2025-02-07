namespace ECommerce.DataAccess.Entities.PaymentSession
{
    public class PaymentSessionEntity
    {
        public string PaymentSessionId { get; set; }
        public Guid UserId { get; set; }
        // TODO: get rid of magical strings
        public string PaymentSessionType { get; set; }

        public PaymentSessionEntity(string paymentSessionId, Guid userId, string paymentSessionType)
        {
            PaymentSessionId = paymentSessionId;
            UserId = userId;
            PaymentSessionType = paymentSessionType;
        }
    }
}