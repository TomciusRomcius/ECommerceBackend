using OrderService.Payment;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public class GeneratePaymentSessionOptions
{
    public required string UserId { get; set; }
    public required int PriceCents { get; set; }
    public required PaymentProvider PaymentProvider { get; set; }
}

public interface IPaymentSessionService
{
    /// <returns>Payment intent json</returns>
    public Task<Result<PaymentSessionModel>> GeneratePaymentSessionAsync(GeneratePaymentSessionOptions sessionOptions);
    public Task<Result<PaymentSessionModel?>> GetPaymentSessionAsync(Guid userId);
}