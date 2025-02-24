using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;
using ECommerce.Domain.Repositories.StoreLocation;

namespace ECommerce.Infrastructure.Repositories.StoreLocation
{
    public class StoreLocationRepository : IStoreLocationRepository
    {
        readonly IPostgresService _postgresService;

        public StoreLocationRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<StoreLocationEntity?> CreateAsync(CreateStoreLocationModel storeLocation)
        {
            string query = @"
                INSERT INTO storeLocations (displayName, address)
                VALUES ($1, $2)
                RETURNING storeLocationId;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(storeLocation.DisplayName),
                new QueryParameter(storeLocation.Address)
            ];

            object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

            if (id is int)
            {
                var result = new StoreLocationEntity(Convert.ToInt32(id), storeLocation.DisplayName, storeLocation.Address);
                return result;
            }

            else return null;
        }

        public async Task DeleteAsync(int storeLocationId)
        {
            string query = @"
                DELETE FROM storeLocations WHERE storeLocationId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(storeLocationId),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<StoreLocationEntity?> FindByIdAsync(int storeLocationId)
        {
            string query = @"
                SELECT * FROM storeLocations WHERE storeLocationId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(storeLocationId),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            StoreLocationEntity? result = null;

            if (rows.Count == 1)
            {
                // TODO: null safety
                var row = rows[0];

                result = new StoreLocationEntity(
                    row.GetColumn<int>("storelocationid"),
                    row.GetColumn<string>("displayname"),
                    row.GetColumn<string>("address")
                );
            }

            return result;
        }

        public async Task<StoreLocationEntity?> FindByNameAsync(string storeLocationName)
        {
            string query = @"
                SELECT * FROM storeLocations WHERE displayname = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(storeLocationName),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            StoreLocationEntity? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];

                result = new StoreLocationEntity(
                    row.GetColumn<int>("storelocationid"),
                    row.GetColumn<string>("displayname"),
                    row.GetColumn<string>("address")
                );
            }

            return result;
        }

        public async Task<List<StoreLocationEntity>> GetAll()
        {
            string query = @"
                SELECT * FROM storeLocations;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
            List<StoreLocationEntity> result = new List<StoreLocationEntity>();

            foreach (var row in rows)
            {
                // TODO: null safety
                result.Add(new StoreLocationEntity(
                    row.GetColumn<int>("storelocationid"),
                    row.GetColumn<string>("displayname"),
                    row.GetColumn<string>("address")
                ));
            }

            return result;
        }

        public async Task UpdateAsync(UpdateStoreLocationModel updateModel)
        {
            string query = @"
                UPDATE storeLocations
                SET
                displayName = COALESCE($1, displayName),
                address = COALESCE($2, address)
                WHERE storeLocationId = $3
            ";

            QueryParameter[] parameters = [
                new QueryParameter(updateModel.DisplayName),
                new QueryParameter(updateModel.Address),
                new QueryParameter(updateModel.StoreLocationId),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}