using ECommerce.Application.UseCases.Store.Commands;
using ECommerce.Domain.Repositories.ProductStoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers
{
    public class AddProductToStoreHandler : IRequestHandler<AddProductToStoreCommand>
    {
        readonly IProductStoreLocationRepository _productStoreLocationRepository;

        public AddProductToStoreHandler(IProductStoreLocationRepository productStoreLocationRepository)
        {
            _productStoreLocationRepository = productStoreLocationRepository;
        }

        public async Task Handle(AddProductToStoreCommand request, CancellationToken cancellationToken)
        {
            await _productStoreLocationRepository.AddProductToStore(request.ProductStoreLocation);
        }
    }
}