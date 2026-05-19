namespace PaymentService.Infrastructure.src.Utils;

public class StripeSettings
{
    public required string ApiKey { get; init; }
    public required string WebhookSecret { get; init; }
    public string CheckoutSuccessUrl { get; init; } = "http://localhost:4200/checkout?payment=success";
    public string CheckoutCancelUrl { get; init; } = "http://localhost:4200/checkout?payment=cancelled";

    public StripeSettings()
    {

    }
}