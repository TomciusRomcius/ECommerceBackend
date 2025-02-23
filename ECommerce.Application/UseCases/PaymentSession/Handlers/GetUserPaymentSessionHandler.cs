using ECommerce.Application.UseCases.PaymentSession.Queries;
using ECommerce.Domain.Entities.PaymentSession;
using ECommerce.Domain.Repositories.PaymentSession;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Handlers
{
    public class GetPaymentSessionHandler : IRequestHandler<GetPaymentSessionQuery, PaymentSessionEntity?>
    {
        readonly IPaymentSessionRepository _paymentSessionRepository;

        public GetPaymentSessionHandler(IPaymentSessionRepository paymentSessionRepository)
        {
            _paymentSessionRepository = paymentSessionRepository;
        }

        public async Task<PaymentSessionEntity?> Handle(GetPaymentSessionQuery request, CancellationToken cancellationToken)
        {
            return await _paymentSessionRepository.GetPaymentSession(request.UserId);
        }
    }
}