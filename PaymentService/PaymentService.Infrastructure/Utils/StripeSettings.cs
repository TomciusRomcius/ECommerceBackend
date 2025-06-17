namespace PaymentService.Infrastructure.Utils;

public class StripeSettings
{
    public required string ApiKey { get; init; }
    public required string WebhookSignature { get; init; }

    public StripeSettings()
    {

    }
}