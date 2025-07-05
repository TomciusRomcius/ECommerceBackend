using ECommerce.Domain.src.Models;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Commands;

public record UpdateUserCommand(UpdateUserModel Updator) : IRequest;