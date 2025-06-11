using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Commands;

public record DeletePaymentSessionCommand(Guid UserId) : IRequest;