using Confluent.Kafka;
using ECommerceBackend.EventTypes;
using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Services;
using PaymentService.Application.src.Interfaces;
using PaymentService.Domain.src.Utils;
using PaymentService.Infrastructure.Interfaces;

namespace PaymentService.Infrastructure.Services;

public class ChargeSucceededConsumer : IChargeSucceededConsumer
{
    private readonly ILogger<ChargeSucceededConsumer> _logger;
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly IPaymentSessionPersistenceService _paymentSessionPersistenceService;
    private const string TopicName = "charge-succeeded";

    public ChargeSucceededConsumer(
        ILogger<ChargeSucceededConsumer> logger,
        KafkaConfiguration kafkaConfiguration,
        PaymentSessionPersistenceService paymentSessionPersistenceService
    )
    {
        _logger = logger;
        _kafkaConfiguration = kafkaConfiguration;
        _paymentSessionPersistenceService = paymentSessionPersistenceService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            KafkaEventConsumer consumer = new(_kafkaConfiguration,
                AutoOffsetReset.Latest,
                "payment-service",
                TopicName);

            ChargeSucceededEvent? ev = consumer.Consume<ChargeSucceededEvent>(cancellationToken);
            if (ev is null)
            {
                _logger.LogError("Failed to parse the charge succeeded event!");
                continue;
            }

            ResultError? error = await _paymentSessionPersistenceService.DeleteAsync(new Guid(ev.UserId));
            if (error is not null)
            {
                _logger.LogError("Failed to delete the payment session of the user. Error: {@Error}", error);
            }
        }
    }
}
