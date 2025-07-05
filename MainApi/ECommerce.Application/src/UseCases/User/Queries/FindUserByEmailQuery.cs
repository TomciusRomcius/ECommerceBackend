using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Queries;

public record FindUserByEmailQuery(string Email) : IRequest<UserEntity?>;