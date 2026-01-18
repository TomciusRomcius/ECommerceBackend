using Confluent.Kafka;
using ECommerceBackend.EventTypes;
using ECommerceBackend.Utils.Database;
using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;

namespace StoreService.Application.Services;

public class ChargeSucceededConsumer
{
    private readonly ILogger<ChargeSucceededConsumer> _logger;
    private readonly DatabaseContext _dbContext;
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly HttpClient _httpClient;
    private readonly MicroserviceHosts _serviceHosts;
    
    public ChargeSucceededConsumer(ILogger<ChargeSucceededConsumer> logger,
        DatabaseContext dbContext,
        KafkaConfiguration kafkaConfiguration,
        HttpClient httpClient,
        MicroserviceHosts serviceHosts)
    {
        _logger = logger;
        _dbContext = dbContext;
        _kafkaConfiguration = kafkaConfiguration;
        _httpClient = httpClient;
        _serviceHosts = serviceHosts;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {ConsumerName}", nameof(ChargeSucceededConsumer));
        while (true)
        {
            KafkaEventConsumer consumer = new(_kafkaConfiguration,
                AutoOffsetReset.Earliest,
                "store-service",
                "charge-succeeded"
            );

            ChargeSucceededEvent? chargeEvent = consumer.Consume<ChargeSucceededEvent>(cancellationToken);
            if (chargeEvent == null)
            {
                
            }
            string userId = chargeEvent?.UserId;
            string orderId = chargeEvent?.OrderId;
            _httpClient.GetAsync($"{_serviceHosts.OrderServiceUrl}/order?userId={userId}&orderId={orderId}")
        }
    }
}
