using ECommerce.Domain.Entities.User;
using MediatR;

namespace ECommerce.Application.UseCases.User.Commands
{
    public record CreateUserCommand(UserEntity User) : IRequest;
}