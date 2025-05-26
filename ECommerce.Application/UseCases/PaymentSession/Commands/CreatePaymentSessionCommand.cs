using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.UseCases.PaymentSession.Commands;

public record CreatePaymentSessionCommand(
    Guid UserId,
    string PaymentSessionId,
    PaymentProvider Provider
) : IRequest;