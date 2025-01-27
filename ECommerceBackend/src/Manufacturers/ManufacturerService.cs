using System.Data;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.Manufacturers
{
    public interface IManufacturerService
    {
        public Task<List<ManufacturerModel>> GetAllManufacturers();
        public Task<int> CreateManufacturer(RequestCreateManufacturerDto createManufacturerDto);
    }

    public class ManufacturerService : IManufacturerService
    {
        private readonly IPostgresService _postgresService;
        public ManufacturerService(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }


        public async Task<List<ManufacturerModel>> GetAllManufacturers()
        {
            string query = @"
                SELECT * FROM manufacturers;
            ";

            var rows = await _postgresService.ExecuteAsync(query);

            List<ManufacturerModel> manufacturers = new List<ManufacturerModel>();
            foreach (Dictionary<string, object> row in rows)
            {
                int? manufacturerId = Convert.ToInt32(row["manufacturerid"]);
                string? name = row["name"].ToString();
                if (manufacturerId == null || name == null)
                {
                    throw new DataException("Retrieved manufacturer id or manufacturer name is null");
                }

                manufacturers.Add(new ManufacturerModel(manufacturerId.Value, name));
            }

            return manufacturers;
        }

        public async Task<int> CreateManufacturer(RequestCreateManufacturerDto createManufacturerDto)
        {
            string query = @"
                INSERT INTO manufacturers (name)
                VALUES (@name)
                RETURNING manufacturerId;
            ";

            QueryParameter[] parameters = [new QueryParameter("name", createManufacturerDto.Name)];

            object? res = await _postgresService.ExecuteScalarAsync(query, parameters);

            if (res == null)
            {
                throw new DataException("Creating manufacturer failed");
            }

            int id = Convert.ToInt32(res);
            return id;
        }
    }
}