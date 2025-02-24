using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.Category;
using ECommerce.Domain.Models.Category;
using ECommerce.Domain.Repositories.Category;

namespace ECommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        IPostgresService _postgresService;

        public CategoryRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<CategoryEntity?> CreateAsync(string categoryName)
        {
            string query = @"
            INSERT INTO categories (name) 
            VALUES ($1)
            RETURNING categoryId;
        ";

            QueryParameter[] parameters = [new QueryParameter(categoryName)];

            object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

            CategoryEntity? result = null;

            if (id is int)
            {
                result = new CategoryEntity(Convert.ToInt32(id), categoryName);
            }

            return result;
        }

        public Task DeleteAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryEntity?> FindByIdAsync(int categoryId)
        {
            string query = @"
                SELECT * FROM categories WHERE categoryId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(categoryId)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            CategoryEntity? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];
                result = new CategoryEntity(categoryId, row.GetColumn<string>("name"));
            }

            return result;
        }

        public async Task<CategoryEntity?> FindByNameAsync(string categoryName)
        {
            string query = @"
                SELECT * FROM categories WHERE name = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(categoryName)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            CategoryEntity? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];
                result = new CategoryEntity(row.GetColumn<int>("categoryid"), categoryName);
            }

            return result;
        }

        public async Task<List<CategoryEntity>> GetAll()
        {
            string query = @"
                SELECT * FROM categories;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
            List<CategoryEntity> result = new List<CategoryEntity>();
            foreach (var row in rows)
            {
                result.Add(
                    new CategoryEntity(row.GetColumn<int>("categoryid"), row.GetColumn<string>("name"))
                );
            }

            return result;
        }

        public Task UpdateAsync(UpdateCategoryModel updateModel)
        {
            throw new NotImplementedException();
        }
    }
}