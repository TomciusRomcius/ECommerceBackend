using ECommerce.Application.Interfaces;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services;

public class StripeWebhookCoordinatorService : IWebhookCoordinatorService
{
    private readonly IWebhookEventStrategyMap _webhookEventStrategyMap;
    private readonly ILogger<StripeWebhookCoordinatorService> _logger;

    public StripeWebhookCoordinatorService(
        IWebhookEventStrategyMap webhookEventStrategyMap,
        ILogger<StripeWebhookCoordinatorService> logger)
    {
        _webhookEventStrategyMap = webhookEventStrategyMap;
        _logger = logger;
    }

    public async Task HandlePaymentWebhook(string json)
    {
        Result<IStripeWebhookStrategy> webhookHandlerStrategyResult = _webhookEventStrategyMap
            .TryGetStrategy<IStripeWebhookStrategy>(paymentEvent.Type);
        if (webhookHandlerStrategyResult.Errors.Any())
        {
            ResultError firstError = webhookHandlerStrategyResult.Errors.First();
            if (firstError.ErrorType == ResultErrorType.UNSUPPORTED
                && webhookHandlerStrategyResult.Errors.Count() == 1)
            {
                _logger.LogWarning(firstError.Message);
            }
            else
            {
                _logger.LogError($"{webhookHandlerStrategyResult.ErrorsToString()}");
            }

            IStripeWebhookStrategy webhookHandlerStrategy = webhookHandlerStrategyResult.GetValue();
            await webhookHandlerStrategy.RunAsync(paymentEvent.Data.Object);
        };
    }
