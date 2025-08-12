using E_Commerce.Bussiness;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class Order_ItemsController : Controller
    {
        [HttpPost("AddOrderItems")]
        public IActionResult AddOrderItems([FromBody] OrderItems OI)
        {
            bool IsAdded = clsOrder_ItemsBussiness.AddOrder_Items(OI.OrderID, OI.ProdcuctID,OI.Quantity,OI.Unit_price);
            if (IsAdded) { return Ok(new { message = "Order Items Added" }); }
            else { return BadRequest(new { message = "Error while adding" }); }
        }
        public class OrderItems
        {
            public int OrderID { get; set; } = 0;

            public int ProdcuctID { get; set; } = 0;

            public int Quantity { get; set; } = 0;

            public int Unit_price { get; set; } = 0;
        }
    }
}
