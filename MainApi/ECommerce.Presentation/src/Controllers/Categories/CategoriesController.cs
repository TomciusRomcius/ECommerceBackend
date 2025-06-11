using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using ECommerce.Presentation.src.Controllers.Categories.dtos;
using ECommerce.Presentation.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.src.Controllers.Categories;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        List<CategoryEntity> categories = await _categoriesService.GetAllCategories();
        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRATOR")]
    public async Task<IActionResult> CreateCategory([FromBody] RequestCreateCategoryDto createCategoryDto)
    {
        Result<int> result = await _categoriesService.CreateCategory(createCategoryDto);
        if (result.Errors.Any())
        {
            return ControllerUtils.ResultErrorToResponse(result.Errors.First());
        }

        int categoryId = result.GetValue();
        return Created(nameof(CreateCategory), categoryId);
    }
}