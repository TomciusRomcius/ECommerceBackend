using ECommerce.Application.EventTypes;
using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services.Consumers
{
    public class ChargeSucceededConsumer : BackgroundService
    {
        private KafkaEventConsumer _consumer;
        private IMediator _mediator;
        private ILogger _logger;

        public ChargeSucceededConsumer(KafkaConfiguration kafkaConfiguration, IMediator mediator, ILogger<ChargeSucceededConsumer> logger)
        {
            _consumer = new KafkaEventConsumer(
                kafkaConfiguration,
                Confluent.Kafka.AutoOffsetReset.Earliest,
                "main-api",
                "charge-succeeded"
            );
            _mediator = mediator;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ChargeSucceededEvent? ev = _consumer.Consume<ChargeSucceededEvent>(stoppingToken);
                    if (ev != null)
                    {
                        _logger.LogInformation(ev.UserId);
                        await _mediator.Publish(ev);
                    }
                    else
                    {
                        _logger.LogError("ChargeSucceededConsumer: failed to parse ChargeSucceededEvent");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError("An exception was caught in ChargeSucceededConsumer: {}", ex.Message);
                }
            }
        }
    }
}
