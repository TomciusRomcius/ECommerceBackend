using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Handlers;

public class CreatePaymentSessionHandler : IRequestHandler<CreatePaymentSessionCommand>
{
    private readonly IPaymentSessionRepository _paymentSessionRepository;

    public CreatePaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
    {
        _paymentSessionRepository = paymentSessionRepository;
    }

    public async Task Handle(CreatePaymentSessionCommand request, CancellationToken cancellationToken)
    {
        // TODO: use provider enum
        await _paymentSessionRepository.CreatePaymentSessionAsync(
            new PaymentSessionEntity(request.PaymentSessionId, request.UserId, "stripe")
        );
    }
}