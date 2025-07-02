using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Application.Utils;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Services.Order;
using ECommerce.Domain.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IMediator _mediator;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    private readonly IOrderValidator _orderValidator;
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;
    private readonly IPaymentSessionFactory _paymentSessionFactory;
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public OrderService(
        ILogger<OrderService> logger,
        IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig,
        IPaymentSessionFactory paymentSessionFactory,
        IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator,
        IOrderValidator orderValidator, IOrderPriceCalculator orderPriceCalculator)
    {
        _logger = logger;
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
        _paymentSessionFactory = paymentSessionFactory;
        _productStoreLocationRepository = productStoreLocationRepository;
        _mediator = mediator;
        _orderValidator = orderValidator;
        _orderPriceCalculator = orderPriceCalculator;
    }

    public async Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
    {
        _logger.LogDebug("Creating order payment session. UserId: {}", userId);

        // TODO: check if payment session already exists

        Result<List<CartProductModel>> itemsResult = await _mediator.Send(new GetUserCartItemsDetailedQuery(userId));
        if (itemsResult.Errors.Any())
        {
            // TODO: handle
            return null;
        }

        List<CartProductModel> items = itemsResult.GetValue();

        List<(int, int)> idTuple = items.Select(item => (item.StoreLocationId, item.ProductId)).ToList();
        List<ProductStoreLocationEntity> products =
            await _productStoreLocationRepository.GetProductsFromStoreAsync(idTuple);

        // Throws an exception on error
        _orderValidator.Validate(items, products);

        decimal price = _orderPriceCalculator.CalculatePrice(items);

        // Create payment session. TODO: extract to separate method
        var client = new HttpClient();
        var createSessionJson = new StringContent(JsonUtils.Serialize(new
        {
            UserId = userId,
            Price = price * 100 // Convert to cents
        }));

        // TODO: gRPC, protect route using JWT
        // TODO: handle errors
        HttpResponseMessage sessionRes = await client
            .PostAsync($"{_microserviceNetworkConfig.PaymentServiceUrl}/paymentsession", createSessionJson);
        PaymentProviderSession? intentSession = JsonSerializer.Deserialize<PaymentProviderSession>(sessionRes.Content.ReadAsStream());

        _logger.LogDebug("Received payment session: {}", intentSession);
        if (intentSession == null)
        {
            // TODO: handle
            _logger.LogError("Received payment session is null");
        }

        return intentSession;
    }

    public async Task OnCharge(Guid userId)
    {
        _logger.LogDebug("Running OnCharge: {}", userId);

        Result<List<CartProductEntity>> cartItemsResult = await _mediator.Send(new GetUserCartItemsQuery(userId));
        if (cartItemsResult.Errors.Any())
        {
            // TODO: handle
            return;
        }

        await _productStoreLocationRepository.UpdateStock(cartItemsResult.GetValue());
        await _mediator.Send(new EraseUserCartCommand(userId));
        await _mediator.Send(new DeletePaymentSessionCommand(userId));
    }
}