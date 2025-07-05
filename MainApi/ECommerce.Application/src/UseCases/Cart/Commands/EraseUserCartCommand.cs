using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Commands;

public record EraseUserCartCommand(Guid UserId) : IRequest;