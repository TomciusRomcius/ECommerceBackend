using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IPostgresService _postgresService;

    public CategoryRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task<CategoryEntity?> CreateAsync(string categoryName)
    {
        var query = @"
            INSERT INTO categories (name) 
            VALUES ($1)
            RETURNING categoryId;
        ";

        QueryParameter[] parameters = [new(categoryName)];

        object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

        CategoryEntity? result = null;

        if (id is int) result = new CategoryEntity(Convert.ToInt32(id), categoryName);

        return result;
    }

    public Task DeleteAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryEntity?> FindByIdAsync(int categoryId)
    {
        var query = @"
                SELECT * FROM categories WHERE categoryId = $1;
            ";

        QueryParameter[] parameters = [new(categoryId)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        CategoryEntity? result = null;

        if (rows.Count == 1)
        {
            Dictionary<string, object> row = rows[0];
            result = new CategoryEntity(categoryId, row.GetColumn<string>("name"));
        }

        return result;
    }

    public async Task<CategoryEntity?> FindByNameAsync(string categoryName)
    {
        var query = @"
                SELECT * FROM categories WHERE name = $1;
            ";

        QueryParameter[] parameters = [new(categoryName)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        CategoryEntity? result = null;

        if (rows.Count == 1)
        {
            Dictionary<string, object> row = rows[0];
            result = new CategoryEntity(row.GetColumn<int>("categoryid"), categoryName);
        }

        return result;
    }

    public async Task<List<CategoryEntity>> GetAll()
    {
        var query = @"
                SELECT * FROM categories;
            ";

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
        List<CategoryEntity> result = new List<CategoryEntity>();
        foreach (Dictionary<string, object> row in rows)
            result.Add(
                new CategoryEntity(row.GetColumn<int>("categoryid"), row.GetColumn<string>("name"))
            );

        return result;
    }

    public Task UpdateAsync(UpdateCategoryModel updateModel)
    {
        throw new NotImplementedException();
    }
}