using ECommerce.DataAccess.Models.Category;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;
using ECommerce.DataAccess.Utils.DictionaryExtensions;

namespace ECommerce.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        IPostgresService _postgresService;

        public CategoryRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<CategoryModel?> CreateAsync(string categoryName)
        {
            string query = @"
            INSERT INTO categories (name) 
            VALUES ($1)
            RETURNING categoryId;
        ";

            QueryParameter[] parameters = [new QueryParameter(categoryName)];

            object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

            CategoryModel? result = null;

            if (id is int)
            {
                result = new CategoryModel(Convert.ToInt32(id), categoryName);
            }

            return result;
        }

        public Task DeleteAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryModel?> FindByIdAsync(int categoryId)
        {
            string query = @"
                SELECT * FROM categories WHERE categoryId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(categoryId)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            CategoryModel? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];
                result = new CategoryModel(categoryId, row.GetColumn<string>("name"));
            }

            return result;
        }

        public async Task<CategoryModel?> FindByNameAsync(string categoryName)
        {
            string query = @"
                SELECT * FROM categories WHERE name = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(categoryName)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            CategoryModel? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];
                result = new CategoryModel(row.GetColumn<int>("categoryid"), categoryName);
            }

            return result;
        }

        public async Task<List<CategoryModel>> GetAll()
        {
            string query = @"
                SELECT * FROM categories;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);
            List<CategoryModel> result = new List<CategoryModel>();
            foreach (var row in rows)
            {
                result.Add(
                    new CategoryModel(row.GetColumn<int>("categoryid"), row.GetColumn<string>("name"))
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