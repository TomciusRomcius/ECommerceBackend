using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Interfaces.Services;

public class GeneratePaymentSessionOptions
{
    public required string UserId { get; set; }
    public required int PriceCents { get; set; }
    public required PaymentProvider PaymentProvider { get; set; }
}

public interface IPaymentSessionService
{
    /// <returns>Payment intent json</returns>
    public Task<Result<PaymentProviderSession>> GeneratePaymentSessionAsync(GeneratePaymentSessionOptions sessionOptions);
    public Task<Result<PaymentProviderSession?>> GetPaymentSessionAsync(Guid userId);
}