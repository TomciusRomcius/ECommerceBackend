using ECommerce.Domain.Entities.Category;
using ECommerce.Domain.Repositories.Category;

namespace ECommerce.Categories
{
    public interface ICategoriesService
    {
        public Task<List<CategoryEntity>> GetAllCategories();
        /// <summary>
        /// Returns a list of ids
        /// </summary>
        public Task<CategoryEntity?> CreateCategory(RequestCreateCategoryDto createCategoryDto);
    }

    public class CategoriesService : ICategoriesService
    {
        readonly ICategoryRepository _categoryRepository;

        public CategoriesService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryEntity>> GetAllCategories()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<CategoryEntity?> CreateCategory(RequestCreateCategoryDto createCategoryDto)
        {
            return await _categoryRepository.CreateAsync(
                createCategoryDto.Name
            );
        }
    }
}
