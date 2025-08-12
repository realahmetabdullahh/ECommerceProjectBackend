using E_Commerce.Business;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            DataTable categories = clsCategoriesBusiness.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("GetCategory{categoryName}") ]
        public IActionResult GetCategory(string categoryName)
        {
            var category = clsCategoriesBusiness.ReadCategoryInfo(categoryName);
            if (category == null)
                return NotFound("Category not found.");
            return Ok(category);
        }

        [HttpPost ("CreateCategory")]
        public IActionResult CreateCategory([FromBody] CategoryCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = clsCategoriesBusiness.AddNewCategory(dto.CategoryName, dto.CategoryDescription);
            if (!success)
                return StatusCode(500, "Failed to add category.");

            return Ok("Category created successfully.");
        }

        [HttpDelete("DeleteCategory{categoryName}")]
        public IActionResult DeleteCategory(string categoryName)
        {
            bool success = clsCategoriesBusiness.DeleteCategory(categoryName);
            if (!success)
                return NotFound("Category not found.");
            return Ok("Category deleted successfully.");
        }
    }

    public class CategoryCreateDTO
    {
        public string CategoryName { get; set; } = "";
        public string? CategoryDescription { get; set; }
    }
}
