using ECommerce.Application.UseCases.StoreLocation.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Handlers;

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