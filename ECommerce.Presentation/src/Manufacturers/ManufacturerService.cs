using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Repositories.Manufacturer;

namespace ECommerce.Manufacturers
{
    public interface IManufacturerService
    {
        public Task<List<ManufacturerEntity>> GetAllManufacturers();
        public Task<ManufacturerEntity?> CreateManufacturer(RequestCreateManufacturerDto createManufacturerDto);
    }

    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<List<ManufacturerEntity>> GetAllManufacturers()
        {
            return await _manufacturerRepository.GetAll();
        }

        public async Task<ManufacturerEntity?> CreateManufacturer(RequestCreateManufacturerDto createManufacturerDto)
        {
            return await _manufacturerRepository.CreateAsync(createManufacturerDto.Name);
        }
    }
}