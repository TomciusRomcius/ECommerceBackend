using ECommerce.Application.src.EventTypes;
using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerce.Application.src.Services.Consumers
{
    public class ChargeSucceededConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly KafkaEventConsumer _consumer;
        private readonly ILogger _logger;

        public ChargeSucceededConsumer(IServiceScopeFactory serviceScopeFactory,
            IOptions<KafkaConfiguration> kafkaConfiguration,
            ILogger<ChargeSucceededConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _consumer = new KafkaEventConsumer(
                kafkaConfiguration.Value,
                Confluent.Kafka.AutoOffsetReset.Earliest,
                "main-api",
                "charge-succeeded"
            );
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Started ExecuteAsync");

            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        ChargeSucceededEvent? ev = _consumer.Consume<ChargeSucceededEvent>(stoppingToken);
                        if (ev != null)
                        {
                            _logger.LogDebug("Parsed incoming event");
                            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                            {
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                await mediator.Publish(ev);
                            }
                        }
                        else
                        {
                            _logger.LogError("Failed to parse event");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("An exception was caught: {}", ex);
                    }
                }
            }, stoppingToken);
        }
    }
}
