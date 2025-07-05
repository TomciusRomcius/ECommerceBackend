using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Commands;

public record CreateUserCommand(UserEntity User) : IRequest;