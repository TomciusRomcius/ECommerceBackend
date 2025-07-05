using ECommerce.Application.src.UseCases.StoreLocation.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Handlers;

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