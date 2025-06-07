namespace ECommerce.Application.Utils
{
    public class WebhookMetadata
    {
        public string? UserId { get; init; }
        public required string EventType { get; init; }
    }
}
