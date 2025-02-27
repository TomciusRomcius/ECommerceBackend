using ECommerce.Domain.Entities.User;
using MediatR;

namespace ECommerce.Application.UseCases.User.Queries
{
    public record FindUserByIdQuery(Guid UserId) : IRequest<UserEntity>;
}