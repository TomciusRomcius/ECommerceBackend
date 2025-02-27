using ECommerce.Domain.Models.StoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Commands
{
    public record UpdateStoreLocationCommand(UpdateStoreLocationModel Updator) : IRequest;
}