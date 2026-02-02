using Confluent.Kafka;
using ECommerceBackend.EventTypes;
using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using StoreService.Application.Interfaces;
using StoreService.Application.UseCases.Store.Commands;
using StoreService.Domain.Entities;

namespace StoreService.Application.Services;

public class ChargeSucceededConsumer : IChargeSucceededConsumer
{
    private readonly ILogger<ChargeSucceededConsumer> _logger;
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly OrderDetailsService _orderDetailsService;
    private readonly IMediator  _mediator;

    public ChargeSucceededConsumer(ILogger<ChargeSucceededConsumer> logger,
        KafkaConfiguration kafkaConfiguration,
        OrderDetailsService orderDetailsService,
        IMediator mediator)
    {
        _logger = logger;
        _kafkaConfiguration = kafkaConfiguration;
        _orderDetailsService = orderDetailsService;
        _mediator = mediator;
    }

    public async Task TryConsumeAndHandle(CancellationToken cancellationToken)
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
                return;
            }
            
            GetOrdersResponseType response = await _orderDetailsService.FetchAsync(chargeEvent.UserId, chargeEvent.OrderId, cancellationToken);
            List<ProductStoreLocationEntity> updater = response.OrderProducts
                .Select(x => new ProductStoreLocationEntity(x.StoreLocationId, x.ProductId, x.Quantity))
                .ToList();
            await _mediator.Send(new UpdateProductsStockCommand(updater), cancellationToken);
        }
    }
}
