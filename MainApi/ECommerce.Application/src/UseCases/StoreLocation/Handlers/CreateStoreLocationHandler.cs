using ECommerce.Application.src.UseCases.StoreLocation.Commands;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Handlers;

public class CreateStoreLocationHandler : IRequestHandler<CreateStoreLocationCommand, StoreLocationEntity?>
{
    private readonly IStoreLocationRepository _storeLocationRepository;

    public CreateStoreLocationHandler(IStoreLocationRepository storeLocationRepository)
    {
        _storeLocationRepository = storeLocationRepository;
    }

    public async Task<StoreLocationEntity?> Handle(CreateStoreLocationCommand request,
        CancellationToken cancellationToken)
    {
        return await _storeLocationRepository.CreateAsync(request.StoreLocation);
    }
}