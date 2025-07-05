using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Queries;

public record FindUserByIdQuery(Guid UserId) : IRequest<UserEntity>;