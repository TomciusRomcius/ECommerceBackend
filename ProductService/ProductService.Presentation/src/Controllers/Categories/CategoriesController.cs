using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Services;
using ProductService.Domain.Entities;
using ProductService.Presentation.Controllers.Categories.Dtos;
using ProductService.Presentation.Utils;

namespace ProductService.Presentation.Controllers.Categories;

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
        var categories = await _categoriesService.GetAllCategories();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] RequestCreateCategoryDto createCategoryDto)
    {
        var result = await _categoriesService.CreateCategory(new CategoryEntity(createCategoryDto.Name));
        if (result.Errors.Any()) return ControllerUtils.ResultErrorToResponse(result.Errors.First());

        var categoryId = result.GetValue();
        return Created(nameof(CreateCategory), new
        {
            categoryId
        });
    }
}