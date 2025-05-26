using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Commands;

public record UpdateStoreLocationCommand(UpdateStoreLocationModel Updator) : IRequest;