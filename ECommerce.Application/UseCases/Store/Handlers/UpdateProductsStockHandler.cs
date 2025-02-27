using ECommerce.Application.UseCases.Store.Commands;
using ECommerce.Domain.Repositories.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers
{
    public class UpdateProductsStockHandler : IRequestHandler<UpdateProductStockCommand>
    {
        readonly IProductStoreLocationRepository _productStoreLocationRepository;

        public UpdateProductsStockHandler(IProductStoreLocationRepository productStoreLocationRepository)
        {
            _productStoreLocationRepository = productStoreLocationRepository;
        }

        public async Task Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            await _productStoreLocationRepository.UpdateProduct(request.ProductStoreLocation);
        }
    }
}