using ECommerce.Application.UseCases.Order.Commands;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Order.Handlers;

public class CreateOrderPaymentSessionHandler : IRequestHandler<CreateOrderPaymentSessionCommand, PaymentSessionEntity>
{
    private readonly IMediator _mediator;

    public async Task<PaymentSessionEntity> Handle(CreateOrderPaymentSessionCommand request,
        CancellationToken cancellationToken)
    {
        return null;
    }
}