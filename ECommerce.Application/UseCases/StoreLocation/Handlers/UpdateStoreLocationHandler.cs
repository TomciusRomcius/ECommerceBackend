using ECommerce.Application.UseCases.StoreLocation.Commands;
using ECommerce.Domain.Repositories.StoreLocation;
using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Handlers
{
    public class UpdateStoreLocationHandler : IRequestHandler<UpdateStoreLocationCommand>
    {
        readonly IStoreLocationRepository _storeLocationRepository;

        public UpdateStoreLocationHandler(IStoreLocationRepository storeLocationRepository)
        {
            _storeLocationRepository = storeLocationRepository;
        }

        public async Task Handle(UpdateStoreLocationCommand request, CancellationToken cancellationToken)
        {
            await _storeLocationRepository.UpdateAsync(request.Updator);
        }
    }
}