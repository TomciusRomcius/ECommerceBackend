using ECommerce.Domain.Entities.User;
using MediatR;

namespace ECommerce.Application.UseCases.User.Queries
{
    public record FindUserByEmailQuery(string Email) : IRequest<UserEntity?>;
}