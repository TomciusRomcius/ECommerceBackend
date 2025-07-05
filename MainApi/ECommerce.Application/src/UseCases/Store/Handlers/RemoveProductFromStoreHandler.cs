using ECommerce.Application.src.UseCases.Store.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class RemoveProductFromStoreHandler : IRequestHandler<RemoveProductFromStoreCommand>
{
    private readonly IProductStoreLocationRepository _productStoreLocationRepository;

    public RemoveProductFromStoreHandler(IProductStoreLocationRepository productStoreLocationRepository)
    {
        _productStoreLocationRepository = productStoreLocationRepository;
    }

    public async Task Handle(RemoveProductFromStoreCommand request, CancellationToken cancellationToken)
    {
        await _productStoreLocationRepository.RemoveProductFromStore(request.StoreLocationId, request.ProductId);
    }
}