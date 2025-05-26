using ECommerce.Domain.Models.PaymentSession;

namespace ECommerce.Domain.Interfaces.Services;

public class GeneratePaymentSessionOptions
{
    public required string UserId { get; set; }

    /// <summary>
    ///     Price in cents
    /// </summary>
    public required int Price { get; set; }
    // TODO: get rid of magic strings
}

public interface IPaymentSessionService
{
    public Task<PaymentProviderSession> GeneratePaymentSession(GeneratePaymentSessionOptions sessionOptions);
    public Task<PaymentProviderEvent> ParseWebhookEvent(string json, string signature);
}