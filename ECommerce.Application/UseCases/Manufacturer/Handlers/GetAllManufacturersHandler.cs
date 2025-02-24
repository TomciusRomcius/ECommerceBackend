using ECommerce.Application.UseCases.Manufacturer.Queries;
using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Repositories.Manufacturer;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Handlers
{
    public class GetAllManufacturersHandler : IRequestHandler<GetAllManufacturersQuery, List<ManufacturerEntity>>
    {
        readonly IManufacturerRepository _manufacturerRepository;

        public GetAllManufacturersHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<List<ManufacturerEntity>> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
        {
            return await _manufacturerRepository.GetAll();
        }
    }
}