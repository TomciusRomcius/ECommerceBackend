using PaymentService.Domain.Models;
using PaymentService.Domain.Utils;

namespace PaymentService.Application.Interfaces;

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
    public Task<Result<T>> ParseWebhookEvent<T>(string json, string signature);
}