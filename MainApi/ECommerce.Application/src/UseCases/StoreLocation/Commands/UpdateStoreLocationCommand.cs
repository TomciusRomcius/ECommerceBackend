using ECommerce.Domain.src.Models;
using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Commands;

public record UpdateStoreLocationCommand(UpdateStoreLocationModel Updator) : IRequest;