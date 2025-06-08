using ECommerce.Domain.Entities;
using ECommerce.Presentation.src.Controllers.Categories.dtos;
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
        CategoryEntity? res = await _categoriesService.CreateCategory(createCategoryDto);
        return Created(nameof(CreateCategory), res);
    }
}