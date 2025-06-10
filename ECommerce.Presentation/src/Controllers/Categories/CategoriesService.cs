using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using ECommerce.Presentation.src.Controllers.Categories.dtos;

namespace ECommerce.Presentation.src.Controllers.Categories;

public interface ICategoriesService
{
    public Task<List<CategoryEntity>> GetAllCategories();
    public Task<Result<int>> CreateCategory(RequestCreateCategoryDto createCategoryDto);
}

public class CategoriesService : ICategoriesService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryEntity>> GetAllCategories()
    {
        return await _categoryRepository.GetAll();
    }

    public async Task<Result<int>> CreateCategory(RequestCreateCategoryDto createCategoryDto)
    {
        return await _categoryRepository.CreateAsync(
            createCategoryDto.Name
        );
    }
}