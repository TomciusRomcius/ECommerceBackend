using System.Data;
using System.Text;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.Categories
{
    public interface ICategoriesService
    {
        public Task<List<CategoryModel>> GetAllCategories();
        /// <summary>
        /// Returns a list of ids
        /// </summary>
        public Task<List<int>> CreateCategories(RequestCreateCategoriesDto createCategoriesDto);
    }

    public class CategoriesService : ICategoriesService
    {
        private readonly IPostgresService _postgresService;

        public CategoriesService(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<List<CategoryModel>> GetAllCategories()
        {
            string query = @"
                SELECT * FROM 1;
            ";

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query);

            List<CategoryModel> result = new List<CategoryModel>();
            foreach (var row in rows)
            {
                int? categoryId = Convert.ToInt32(row["categoryid"]);
                string? name = row["name"].ToString();

                if (categoryId == null || name == null)
                {
                    throw new DataException("categoryid or name is null!");
                }

                result.Add(new CategoryModel(categoryId.Value, name));
            }

            return result;
        }

        public async Task<List<int>> CreateCategories(RequestCreateCategoriesDto createCategoriesDto)
        {
            StringBuilder query = new StringBuilder();
            List<QueryParameter> parameters = new List<QueryParameter>();

            query.AppendLine("WITH inserted AS(");
            query.AppendLine("INSERT INTO categories(name)");
            query.AppendLine("VALUES ");

            int i = 0;
            foreach (CategoryDto categoryDto in createCategoriesDto.Categories)
            {
                string queryLine = $"(@name{i}),";
                query.Append(queryLine);

                parameters.Add(new QueryParameter($"name{i}", categoryDto.Name));

                i++;
            }

            // Remove trailing comma
            query.Remove(query.Length - 1, 1);

            query.AppendLine("RETURNING categoryId");
            query.AppendLine(")");
            query.AppendLine("SELECT categoryId FROM inserted;");

            // TODO: remove double list initialization
            var rows = await _postgresService.ExecuteAsync(query.ToString(), parameters.ToArray());
            List<int> idList = new List<int>();
            foreach (var row in rows)
            {
                idList.Add(Convert.ToInt32(row["categoryid"]));
            }

            return idList;
        }
    }
}
