using MediatR;

namespace ECommerce.Application.UseCases.Cart.Commands;

public record EraseUserCartCommand(Guid UserId) : IRequest;