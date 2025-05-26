using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.User.Queries;

public record FindUserByEmailQuery(string Email) : IRequest<UserEntity?>;