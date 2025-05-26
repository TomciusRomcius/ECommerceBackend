using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.UseCases.User.Commands;

public record UpdateUserCommand(UpdateUserModel Updator) : IRequest;