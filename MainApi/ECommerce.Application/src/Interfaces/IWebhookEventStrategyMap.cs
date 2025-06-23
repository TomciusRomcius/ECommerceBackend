using ECommerce.Domain.src.Utils;

namespace ECommerce.Application.src.Interfaces
{
    public interface IWebhookEventStrategyMap
    {
        void AddStrategy<T>(string eventType, T strategy);
        Result<T> TryGetStrategy<T>(string strategyName);
    }
}