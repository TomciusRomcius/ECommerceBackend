using ECommerce.Application.Interfaces;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Services
{
    public class WebhookEventStrategyMapContainer
    {
        private Dictionary<PaymentProvider, IWebhookEventStrategyMap> _strategyMaps;

        public WebhookEventStrategyMapContainer(Dictionary<PaymentProvider, IWebhookEventStrategyMap> strategyMaps)
        {
            _strategyMaps = strategyMaps;
        }

        public IWebhookEventStrategyMap? GetStrategyMap(PaymentProvider paymentProvider)
        {
            _strategyMaps.TryGetValue(paymentProvider, out var strategyMap);
            return strategyMap;
        }
    }
}
