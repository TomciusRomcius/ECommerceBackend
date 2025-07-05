using ECommerce.Application.src.UseCases.Store.Commands;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class AddProductToStoreHandler : IRequestHandler<AddProductToStoreCommand, ResultError?>
{
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public AddProductToStoreHandler(IProductStoreLocationRepository productStoreLocationRepository)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
    }

    public async Task<ResultError?> Handle(AddProductToStoreCommand request, CancellationToken cancellationToken)
    {
        return await _productStoreLocationRepository.AddProductToStore(request.ProductStoreLocation);
    }
}