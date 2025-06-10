using ECommerce.Application.UseCases.Store.Commands;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers;

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