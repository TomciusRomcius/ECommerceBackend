using ECommerce.DataAccess.Models.Manufacturer;
using ECommerce.DataAccess.Repositories;

namespace ECommerce.Manufacturers
{
    public interface IManufacturerService
    {
        public Task<List<ManufacturerModel>> GetAllManufacturers();
        public Task<ManufacturerModel?> CreateManufacturer(RequestCreateManufacturerDto createManufacturerDto);
    }

    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<List<ManufacturerModel>> GetAllManufacturers()
        {
            return await _manufacturerRepository.GetAll();
        }

        public async Task<ManufacturerModel?> CreateManufacturer(RequestCreateManufacturerDto createManufacturerDto)
        {
            return await _manufacturerRepository.CreateAsync(createManufacturerDto.Name);
        }
    }
}