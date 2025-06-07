﻿using ECommerce.Application.Interfaces;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Services
{
    public class WebhookEventStrategyMapContainer
    {
        private Dictionary<PaymentProvider, IWebhookEventStrategyMap> _strategyMaps = [];

        public void AddStrategyMap(PaymentProvider paymentProvider, IWebhookEventStrategyMap strategyMap)
        {
            _strategyMaps.Add(paymentProvider, strategyMap);
        }

        public IWebhookEventStrategyMap? GetStrategyMap(PaymentProvider paymentProvider)
        {
            _strategyMaps.TryGetValue(paymentProvider, out var strategyMap);
            return strategyMap;
        }
    }
}
