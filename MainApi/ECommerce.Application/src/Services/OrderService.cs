using ECommerce.Application.src.Interfaces;
using ECommerce.Application.src.UseCases.Cart.Queries;
using ECommerce.Application.src.Utils;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Enums;
using ECommerce.Domain.src.Interfaces.Services;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Services.Order;
using ECommerce.Domain.src.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerce.Application.src.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IMediator _mediator;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    private readonly IOrderValidator _orderValidator;
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public OrderService(
        ILogger<OrderService> logger,
        IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig,
        IPaymentSessionService paymentSessionService,
        IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator,
        IOrderValidator orderValidator, IOrderPriceCalculator orderPriceCalculator)
    {
        _logger = logger;
        _paymentSessionService = paymentSessionService;
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
        _productStoreLocationRepository = productStoreLocationRepository;
        _mediator = mediator;
        _orderValidator = orderValidator;
        _orderPriceCalculator = orderPriceCalculator;
    }

    public async Task<PaymentSessionModel?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
    {
        _logger.LogTrace("Creating order payment session. UserId: {}", userId);

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

        var intentSession = await _paymentSessionService.GeneratePaymentSessionAsync(
            new GeneratePaymentSessionOptions
            {
                PaymentProvider = paymentProvider,
                PriceCents = Convert.ToInt32(price * 100), // Convert to cents
                UserId = userId.ToString(),
            }
        );

        if (intentSession.Errors.Any())
        {
            // TODO: handle
            _logger.LogError("Failed to create payment intent for user: {}", userId);
            return null;
        }
        else
        {
            _logger.LogDebug("Created payment session: {}", intentSession);
            _logger.LogTrace("Created payment session for user: {}", userId);
        }

        return intentSession.GetValue();
    }
}