using MediatR;

namespace ECommerce.Application.src.UseCases.User.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest;