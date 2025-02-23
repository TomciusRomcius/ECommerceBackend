using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Domain.Repositories.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Handlers
{
    public class DeletePaymentSessionHandler : IRequestHandler<DeletePaymentSessionCommand>
    {
        readonly IPaymentSessionRepository _paymentSessionRepository;

        public DeletePaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
        {
            _paymentSessionRepository = paymentSessionRepository;
        }

        public async Task Handle(DeletePaymentSessionCommand request, CancellationToken cancellationToken)
        {
            await _paymentSessionRepository.DeletePaymentSession(request.UserId);
        }
    }
}