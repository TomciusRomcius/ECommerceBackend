using System.Data;
using ECommerce.DataAccess.Models;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.DataAccess.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        readonly IPostgresService _postgresService;

        public ManufacturerRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<ManufacturerModel?> CreateAsync(string manufacturerName)
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

            return new ManufacturerModel(id, manufacturerName);
        }

        public async Task DeleteAsync(int manufacturerId)
        {
            string query = @"
                DELETE FROM manufacturers WHERE manufacturerId = $1
            ";

            QueryParameter[] parameters = [new QueryParameter(manufacturerId)];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public Task<ManufacturerModel?> FindByIdAsync(int manufacturerId)
        {
            throw new NotImplementedException();
        }

        public Task<ManufacturerModel?> FindByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ManufacturerModel>> GetAll()
        {
            string query = @"
                SELECT * FROM manufacturers;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
            List<ManufacturerModel> result = new List<ManufacturerModel>();

            foreach (var row in rows)
            {
                // TODO: null safety
                result.Add(
                    new ManufacturerModel(Convert.ToInt32(row["manufacturerid"]), row["name"].ToString())
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