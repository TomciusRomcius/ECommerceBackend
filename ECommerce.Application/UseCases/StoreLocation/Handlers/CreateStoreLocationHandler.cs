using ECommerce.Application.UseCases.StoreLocation.Commands;
using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Repositories.StoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Handlers
{
    public class CreateStoreLocationHandler : IRequestHandler<CreateStoreLocationCommand, StoreLocationEntity?>
    {
        readonly IStoreLocationRepository _storeLocationRepository;

        public CreateStoreLocationHandler(IStoreLocationRepository storeLocationRepository)
        {
            _storeLocationRepository = storeLocationRepository;
        }

        public async Task<StoreLocationEntity?> Handle(CreateStoreLocationCommand request, CancellationToken cancellationToken)
        {
            return await _storeLocationRepository.CreateAsync(request.StoreLocation);
        }
    }
}