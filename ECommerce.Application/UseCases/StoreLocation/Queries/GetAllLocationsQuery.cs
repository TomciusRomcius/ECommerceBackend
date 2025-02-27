using ECommerce.Domain.Entities.StoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Queries
{
    public record GetAllLocationsQuery : IRequest<List<StoreLocationEntity>>;
}