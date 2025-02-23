using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Domain.Repositories.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Handlers
{
    public class CreatePaymentSessionHandler : IRequestHandler<CreatePaymentSessionCommand>
    {
        readonly IPaymentSessionRepository _paymentSessionRepository;

        public CreatePaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
        {
            _paymentSessionRepository = paymentSessionRepository;
        }

        public async Task Handle(CreatePaymentSessionCommand request, CancellationToken cancellationToken)
        {
            await _paymentSessionRepository.CreatePaymentSessionAsync(request.PaymentSessionEntity);
        }
    }
}