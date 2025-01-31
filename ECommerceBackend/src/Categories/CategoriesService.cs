using ECommerce.DataAccess.Models.Category;
using ECommerce.DataAccess.Repositories;

namespace ECommerce.Categories
{
    public interface ICategoriesService
    {
        public Task<List<CategoryModel>> GetAllCategories();
        /// <summary>
        /// Returns a list of ids
        /// </summary>
        public Task<CategoryModel?> CreateCategory(RequestCreateCategoryDto createCategoryDto);
    }

    public class CategoriesService : ICategoriesService
    {
        readonly ICategoryRepository _categoryRepository;

        public CategoriesService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryModel>> GetAllCategories()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<CategoryModel?> CreateCategory(RequestCreateCategoryDto createCategoryDto)
        {
            return await _categoryRepository.CreateAsync(
                createCategoryDto.Name
            );
        }
    }
}
