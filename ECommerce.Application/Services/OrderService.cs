using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Application.UseCases.PaymentSession.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Services.Order;
using MediatR;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IMediator _mediator;
    private readonly IOrderPriceCalculator _orderPriceCalculator;
    private readonly IOrderValidator _orderValidator;
    private readonly IPaymentSessionFactory _paymentSessionFactory;
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public OrderService(IPaymentSessionFactory paymentSessionFactory,
        IProductStoreLocationRepository productStoreLocationRepository, IMediator mediator,
        IOrderValidator orderValidator, IOrderPriceCalculator orderPriceCalculator)
    {
        _paymentSessionFactory = paymentSessionFactory;
        _productStoreLocationRepository = productStoreLocationRepository;
        _mediator = mediator;
        _orderValidator = orderValidator;
        _orderPriceCalculator = orderPriceCalculator;
    }

    public async Task<PaymentProviderSession?> CreateOrderPaymentSession(Guid userId, PaymentProvider paymentProvider)
    {
        if (await _mediator.Send(new GetPaymentSessionQuery(userId)) is not null)
            throw new InvalidOperationException(
                "Cannot create a payment session: there is an existing payment session!");

        List<CartProductModel>? items = await _mediator.Send(new GetUserCartItemsDetailedQuery(userId));

        List<(int, int)> idTuple = items.Select(item => (item.StoreLocationId, item.ProductId)).ToList();
        List<ProductStoreLocationEntity> products =
            await _productStoreLocationRepository.GetProductsFromStoreAsync(idTuple);

        // Throws an exception on error
        _orderValidator.Validate(items, products);

        decimal price = _orderPriceCalculator.CalculatePrice(items);

        var paymentOptions = new GeneratePaymentSessionOptions
        {
            UserId = userId.ToString(),
            Price = (int)_orderPriceCalculator.CalculatePrice(items) * 100 // TODO: may be dangerous
        };

        IPaymentSessionService paymentSessionService =
            _paymentSessionFactory.CreatePaymentSessionService(paymentProvider);

        PaymentProviderSession intentSession = await paymentSessionService.GeneratePaymentSession(paymentOptions);

        // TODO: remove hard-coded provider
        await _mediator.Send(new CreatePaymentSessionCommand(userId, intentSession.SessionId, PaymentProvider.STRIPE));

        return intentSession;
    }

    public async Task OnCharge(Guid userId)
    {
        List<CartProductEntity>? cartItems = await _mediator.Send(new GetUserCartItemsQuery(userId));
        await _productStoreLocationRepository.UpdateStock(cartItems);
        await _mediator.Send(new EraseUserCartCommand(userId));
        await _mediator.Send(new DeletePaymentSessionCommand(userId));
    }
}