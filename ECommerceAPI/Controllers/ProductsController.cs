using E_Commerce.Bussiness;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        
        [HttpGet("GetProduct{id}")]
        public IActionResult GetProduct(int id)
        {
            string name = "", description = "", imageUrl = "";
            decimal price = 0;
            int categoryID = 0, userID = 0;
            DateTime createdAt = DateTime.MinValue;

            var product = clsProductsBussiness.GetProductInfo(id, ref name, ref description, ref price, ref categoryID, ref userID, ref imageUrl, ref createdAt);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

    
        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] ProductCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = clsProductsBussiness.AddNewProduct(dto.Name, dto.Description, dto.Price, dto.CategoryID, dto.UserID, dto.ImageURL);

            if (!success)
                return StatusCode(500, "Failed to add the product.");

            return Ok("Product added successfully.");
        }

        [HttpPut("UpdateProduct{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = clsProductsBussiness.UpdateProduct(id, dto.Name, dto.Description ?? string.Empty, dto.Price, dto.CategoryID, dto.UserID, dto.ImageURL ?? string.Empty);

            if (!success)
                return NotFound("Product not found or update failed.");

            return Ok("Product updated successfully.");
        }

        [HttpDelete("^DeleteProduct{id}")]
        public IActionResult DeleteProduct(int id)
        {
            bool success = clsProductsBussiness.DeleteProduct(id);

            if (!success)
                return NotFound("Product not found.");

            return Ok("Product deleted successfully.");
        }
    }

   public class ProductCreateDTO
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public string? ImageURL { get; set; }
    }

    public class ProductUpdateDTO
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public string? ImageURL { get; set; }
    }
}
