using MediatR;

namespace ECommerce.Application.UseCases.Store
{
    public record GetProductIdsFromStoreQuery(int StoreLocationId) : IRequest<List<int>>;
}