using PaymentService.Domain.src.Models;
using PaymentService.Domain.src.Utils;

namespace PaymentService.Application.src.Interfaces;

public class GeneratePaymentSessionOptions
{
    public required Guid UserId { get; set; }

    /// <summary>
    ///     Price in cents
    /// </summary>
    public required int Price { get; set; }
    // TODO: get rid of magic strings
}

public interface IProviderPaymentSessionService
{
    public Task<PaymentProviderSession> GeneratePaymentSession(GeneratePaymentSessionOptions sessionOptions);
    public Task<Result<T>> ParseWebhookEvent<T>(string json, string signature);
}