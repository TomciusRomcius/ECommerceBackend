using ECommerce.Application.src.UseCases.StoreLocation.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Handlers;

public class RemoveStoreLocationHandler : IRequestHandler<RemoveStoreLocationCommand>
{
    private readonly IStoreLocationRepository _storeLocationRepository;

    public RemoveStoreLocationHandler(IStoreLocationRepository storeLocationRepository)
    {
        _storeLocationRepository = storeLocationRepository;
    }

    public async Task Handle(RemoveStoreLocationCommand request, CancellationToken cancellationToken)
    {
        await _storeLocationRepository.DeleteAsync(request.StoreLocationId);
    }
}