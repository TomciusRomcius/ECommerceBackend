using ECommerce.Application.src.EventTypes;
using ECommerce.Application.src.UseCases.Cart.Commands;
using ECommerce.Application.src.UseCases.Cart.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Order.NotificationHandlers;

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