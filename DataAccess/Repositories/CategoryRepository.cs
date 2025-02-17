using ECommerce.DataAccess.Models.Category;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

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

        public Task<CategoryModel?> FindByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryModel?> FindByNameAsync(string categoryName)
        {
            throw new NotImplementedException();
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
                    new CategoryModel(Convert.ToInt32(row["categoryid"]), row["name"].ToString()!)
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