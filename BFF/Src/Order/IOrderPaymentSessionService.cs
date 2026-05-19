namespace BFF.Order;

public interface IOrderPaymentSessionService
{
    Task<HttpResponseMessage> CreateOrderPaymentSessionAsync(
        bool testCharge,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
