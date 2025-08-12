using E_Commerce.Bussiness;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet("GetOrder{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            var order = clsOrdersBussiness.GetOrderInfo(orderId);
            if (order == null)
                return NotFound("Order not found.");
            return Ok(order);
        }

        [HttpGet("GetAllOrder")]
        public IActionResult GetOrders([FromQuery] string? status, [FromQuery] int? overThanThisAmount, [FromQuery] int? userID)
        {
            DataTable orders = clsOrdersBussiness.GetOrderList(status, overThanThisAmount, userID);
            return Ok(orders);
        }

       
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] OrderCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = clsOrdersBussiness.AddNewOrder(dto.UserID, dto.AddressID, DateTime.Now, dto.Status, dto.TotalAmount);
            if (!success)
                return StatusCode(500, "Failed to create order.");

            return Ok("Order created successfully.");
        }

        [HttpDelete("DeleteOrder{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            bool success = clsOrdersBussiness.DeleteOrder(orderId);
            if (!success)
                return NotFound("Order not found.");
            return Ok("Order deleted successfully.");
        }
    }

    public class OrderCreateDTO
    {
        public int UserID { get; set; }
        public int AddressID { get; set; }
        public string Status { get; set; } = "";
        public decimal TotalAmount { get; set; }
    }
}
