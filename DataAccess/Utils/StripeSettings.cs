namespace ECommerce.DataAccess.Utils
{
    public class StripeSettings
    {
        public required string ApiKey { get; set; }
        public required string WebhookSignature { get; set; }
    }
}