using MediatR;

namespace ECommerce.Application.UseCases.User.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest;