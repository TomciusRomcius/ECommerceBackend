using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.UseCases.User.Commands
{
    public record UpdateUserCommand(UpdateUserModel Updator) : IRequest;
}