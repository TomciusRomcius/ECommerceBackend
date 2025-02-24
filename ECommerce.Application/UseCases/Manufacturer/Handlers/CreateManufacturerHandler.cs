using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Repositories.Manufacturer;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Handlers
{
    public class CreateManufacturerHandler : IRequestHandler<CreateManufacturerCommand, ManufacturerEntity>
    {
        readonly IManufacturerRepository _manufacturerRepository;

        public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<ManufacturerEntity> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
        {
            return await _manufacturerRepository.CreateAsync(request.Name);
        }
    }
}