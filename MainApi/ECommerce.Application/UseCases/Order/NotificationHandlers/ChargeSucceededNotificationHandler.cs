using ECommerce.Application.EventTypes;
using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Order.NotificationHandlers;

public class ChargeSucceededNotificationHandler : INotificationHandler<ChargeSucceededEvent>
{
    private readonly IMediator _mediator;
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public ChargeSucceededNotificationHandler(IProductStoreLocationRepository productStoreLocationRepository,
        IMediator mediator)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
        _mediator = mediator;
    }

    public async Task Handle(ChargeSucceededEvent notification, CancellationToken cancellationToken)
    {
        Result<List<CartProductEntity>> cartItemsResult = await _mediator.Send(new GetUserCartItemsQuery(new Guid(notification.UserId)));
        if (cartItemsResult.Errors.Any())
        {
            // TODO: handle
            return;
        }
        await _productStoreLocationRepository.UpdateStock(cartItemsResult.GetValue());
        await _mediator.Send(new EraseUserCartCommand(new Guid(notification.UserId)));
    }
}