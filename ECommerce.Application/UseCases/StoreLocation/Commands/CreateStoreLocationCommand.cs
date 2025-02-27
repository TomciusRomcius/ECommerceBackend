using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Commands
{
    public record CreateStoreLocationCommand(CreateStoreLocationModel StoreLocation) : IRequest<StoreLocationEntity?>;
}