using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class StoreLocationRepository : IStoreLocationRepository
{
    private readonly IPostgresService _postgresService;

    public StoreLocationRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task<StoreLocationEntity?> CreateAsync(CreateStoreLocationModel storeLocation)
    {
        var query = @"
                INSERT INTO storeLocations (displayName, address)
                VALUES ($1, $2)
                RETURNING storeLocationId;
            ";

        QueryParameter[] parameters =
        [
            new(storeLocation.DisplayName),
            new(storeLocation.Address)
        ];

        object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

        if (id is int)
        {
            var result = new StoreLocationEntity(Convert.ToInt32(id), storeLocation.DisplayName, storeLocation.Address);
            return result;
        }

        return null;
    }

    public async Task DeleteAsync(int storeLocationId)
    {
        var query = @"
                DELETE FROM storeLocations WHERE storeLocationId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(storeLocationId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<StoreLocationEntity?> FindByIdAsync(int storeLocationId)
    {
        var query = @"
                SELECT * FROM storeLocations WHERE storeLocationId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(storeLocationId)
        ];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        StoreLocationEntity? result = null;

        if (rows.Count == 1)
        {
            // TODO: null safety
            Dictionary<string, object> row = rows[0];

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
        var query = @"
                SELECT * FROM storeLocations WHERE displayname = $1;
            ";

        QueryParameter[] parameters =
        [
            new(storeLocationName)
        ];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        StoreLocationEntity? result = null;

        if (rows.Count == 1)
        {
            Dictionary<string, object> row = rows[0];

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
        var query = @"
                SELECT * FROM storeLocations;
            ";

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
        List<StoreLocationEntity> result = new List<StoreLocationEntity>();

        foreach (Dictionary<string, object> row in rows)
            // TODO: null safety
            result.Add(new StoreLocationEntity(
                row.GetColumn<int>("storelocationid"),
                row.GetColumn<string>("displayname"),
                row.GetColumn<string>("address")
            ));

        return result;
    }

    public async Task UpdateAsync(UpdateStoreLocationModel updateModel)
    {
        var query = @"
                UPDATE storeLocations
                SET
                displayName = COALESCE($1, displayName),
                address = COALESCE($2, address)
                WHERE storeLocationId = $3
            ";

        QueryParameter[] parameters =
        [
            new(updateModel.DisplayName),
            new(updateModel.Address),
            new(updateModel.StoreLocationId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }
}