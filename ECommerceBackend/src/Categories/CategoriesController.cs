using ECommerce.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Categories
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCategories()
        {
            List<CategoryModel> categories = await _categoriesService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateCategory([FromBody()] RequestCreateCategoryDto createCategoryDto)
        {
            CategoryModel? res = await _categoriesService.CreateCategory(createCategoryDto);
            return Created(nameof(CreateCategory), res);
        }
    }
}