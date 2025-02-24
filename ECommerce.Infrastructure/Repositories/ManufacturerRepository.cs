using System.Data;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Models.Manufacturer;
using ECommerce.Domain.Repositories.Manufacturer;

namespace ECommerce.Infrastructure.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        readonly IPostgresService _postgresService;

        public ManufacturerRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<ManufacturerEntity?> CreateAsync(string manufacturerName)
        {
            string query = @"
                INSERT INTO manufacturers (name)
                VALUES ($1)
                RETURNING manufacturerId;
            ";

            QueryParameter[] parameters = [new QueryParameter(manufacturerName)];

            object? res = await _postgresService.ExecuteScalarAsync(query, parameters);

            if (res == null)
            {
                throw new DataException("Creating manufacturer failed");
            }

            int id = Convert.ToInt32(res);

            return new ManufacturerEntity(id, manufacturerName);
        }

        public async Task DeleteAsync(int manufacturerId)
        {
            string query = @"
                DELETE FROM manufacturers WHERE manufacturerId = $1
            ";

            QueryParameter[] parameters = [new QueryParameter(manufacturerId)];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<ManufacturerEntity?> FindByIdAsync(int manufacturerId)
        {
            string query = @"
                SELECT * FROM manufacturers WHERE manufacturerId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(manufacturerId)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            ManufacturerEntity? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];
                result = new ManufacturerEntity(manufacturerId, row.GetColumn<string>("name"));
            }

            return result;
        }

        public async Task<ManufacturerEntity?> FindByNameAsync(string name)
        {
            string query = @"
                SELECT * FROM manufacturers WHERE name = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(name)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            ManufacturerEntity? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];
                result = new ManufacturerEntity(row.GetColumn<int>("manufacturerid"), name);
            }

            return result;
        }

        public async Task<List<ManufacturerEntity>> GetAll()
        {
            string query = @"
                SELECT * FROM manufacturers;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
            List<ManufacturerEntity> result = new List<ManufacturerEntity>();

            foreach (var row in rows)
            {
                // TODO: null safety
                result.Add(
                    new ManufacturerEntity(
                        row.GetColumn<int>("manufacturerid"),
                        row.GetColumn<string>("name")
                    )
                );
            }

            return result;
        }

        public Task UpdateAsync(UpdateManufacturerModel updateModel)
        {
            throw new NotImplementedException();
        }
    }
}