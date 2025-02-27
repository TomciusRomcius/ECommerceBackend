using ECommerce.Domain.Models.ProductStoreLocation;
using ECommerce.Domain.Repositories.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers
{
    public class GetProductsFromStoreHandler : IRequestHandler<GetProductsFromStoreQuery, List<DetailedProductModel>>
    {
        readonly IProductStoreLocationRepository _productStoreLocationRepository;

        public GetProductsFromStoreHandler(IProductStoreLocationRepository productStoreLocationRepository)
        {
            _productStoreLocationRepository = productStoreLocationRepository;
        }

        public async Task<List<DetailedProductModel>> Handle(GetProductsFromStoreQuery request, CancellationToken cancellationToken)
        {
            return await _productStoreLocationRepository.GetProductsFromStoreAsync(request.StoreLocationId);
        }
    }
}