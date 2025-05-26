using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Application.UseCases.Common.Notifications;
using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Order.NotificationHandlers;

public class ChargeSucceededNotificationHandler : INotificationHandler<ChargeSucceededNotification>
{
    private readonly IMediator _mediator;
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public ChargeSucceededNotificationHandler(IProductStoreLocationRepository productStoreLocationRepository,
        IMediator mediator)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
        _mediator = mediator;
    }

    public async Task Handle(ChargeSucceededNotification notification, CancellationToken cancellationToken)
    {
        List<CartProductEntity> cartItems = await _mediator.Send(new GetUserCartItemsQuery(notification.UserId));
        await _productStoreLocationRepository.UpdateStock(cartItems);
        await _mediator.Send(new EraseUserCartCommand(notification.UserId));
        await _mediator.Send(new DeletePaymentSessionCommand(notification.UserId));
    }
}