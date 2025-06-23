namespace PaymentService.Infrastructure.src.Utils;

public class StripeSettings
{
    public required string ApiKey { get; init; }
    public required string WebhookSecret { get; init; }

    public StripeSettings()
    {

    }
}