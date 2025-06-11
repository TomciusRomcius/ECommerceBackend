using ECommerce.Application.UseCases.StoreLocation.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Handlers;

public class UpdateStoreLocationHandler : IRequestHandler<UpdateStoreLocationCommand>
{
    private readonly IStoreLocationRepository _storeLocationRepository;

    public UpdateStoreLocationHandler(IStoreLocationRepository storeLocationRepository)
    {
        _storeLocationRepository = storeLocationRepository;
    }

    public async Task Handle(UpdateStoreLocationCommand request, CancellationToken cancellationToken)
    {
        await _storeLocationRepository.UpdateAsync(request.Updator);
    }
}