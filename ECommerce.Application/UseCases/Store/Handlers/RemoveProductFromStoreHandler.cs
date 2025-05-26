using ECommerce.Application.UseCases.Store.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.Store.Handlers;

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