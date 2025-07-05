using ECommerce.Application.src.UseCases.Store.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class UpdateProductsStockHandler : IRequestHandler<UpdateProductStockCommand>
{
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public UpdateProductsStockHandler(IProductStoreLocationRepository productStoreLocationRepository)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
    }

    public async Task Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        await _productStoreLocationRepository.UpdateProduct(request.ProductStoreLocation);
    }
}