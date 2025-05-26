using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly IPostgresService _postgresService;

    public ManufacturerRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task<ManufacturerEntity?> CreateAsync(string manufacturerName)
    {
        var query = @"
                INSERT INTO manufacturers (name)
                VALUES ($1)
                RETURNING manufacturerId;
            ";

        QueryParameter[] parameters = [new(manufacturerName)];

        object? res = await _postgresService.ExecuteScalarAsync(query, parameters);

        if (res == null) throw new DataException("Creating manufacturer failed");

        var id = Convert.ToInt32(res);

        return new ManufacturerEntity(id, manufacturerName);
    }

    public async Task DeleteAsync(int manufacturerId)
    {
        var query = @"
                DELETE FROM manufacturers WHERE manufacturerId = $1
            ";

        QueryParameter[] parameters = [new(manufacturerId)];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<ManufacturerEntity?> FindByIdAsync(int manufacturerId)
    {
        var query = @"
                SELECT * FROM manufacturers WHERE manufacturerId = $1;
            ";

        QueryParameter[] parameters = [new(manufacturerId)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        ManufacturerEntity? result = null;

        if (rows.Count == 1)
        {
            Dictionary<string, object> row = rows[0];
            result = new ManufacturerEntity(manufacturerId, row.GetColumn<string>("name"));
        }

        return result;
    }

    public async Task<ManufacturerEntity?> FindByNameAsync(string name)
    {
        var query = @"
                SELECT * FROM manufacturers WHERE name = $1;
            ";

        QueryParameter[] parameters = [new(name)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        ManufacturerEntity? result = null;

        if (rows.Count == 1)
        {
            Dictionary<string, object> row = rows[0];
            result = new ManufacturerEntity(row.GetColumn<int>("manufacturerid"), name);
        }

        return result;
    }

    public async Task<List<ManufacturerEntity>> GetAll()
    {
        var query = @"
                SELECT * FROM manufacturers;
            ";

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
        List<ManufacturerEntity> result = new List<ManufacturerEntity>();

        foreach (Dictionary<string, object> row in rows)
            // TODO: null safety
            result.Add(
                new ManufacturerEntity(
                    row.GetColumn<int>("manufacturerid"),
                    row.GetColumn<string>("name")
                )
            );

        return result;
    }

    public Task UpdateAsync(UpdateManufacturerModel updateModel)
    {
        throw new NotImplementedException();
    }
}