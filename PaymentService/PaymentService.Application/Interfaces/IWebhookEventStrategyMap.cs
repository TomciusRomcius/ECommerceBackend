using PaymentService.Domain.Utils;

namespace PaymentService.Application.Interfaces
{
    public interface IWebhookEventStrategyMap
    {
        void AddStrategy<T>(string eventType, T strategy);
        Result<T> TryGetStrategy<T>(string strategyName);
    }
}