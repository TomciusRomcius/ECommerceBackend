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
            return Ok(new ResponseGetAllCategoriesDto() { Categories = categories });
        }

        [HttpPost()]
        public async Task<IActionResult> CreateCategories([FromBody()] RequestCreateCategoriesDto createCategoriesDto)
        {
            List<int> res = await _categoriesService.CreateCategories(createCategoriesDto);
            return Ok(new ResponseCreateCategoriesDto() { Ids = res });
        }
    }
}