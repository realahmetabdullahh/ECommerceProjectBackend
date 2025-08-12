using E_Commerce.Bussiness;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
  
        [HttpGet("GetAddress{userId}")]
        public IActionResult GetAddress(int userId)
        {
            var addressBussiness = new clsAddressBussiness(0, null, null, null, null, null, null);
            var address = addressBussiness.GetAddressInfo(userId);

            if (address == null)
                return NotFound("Address not found.");

            return Ok(address);
        }

        [HttpGet("GetAllAddresses")]
        public IActionResult GetAllAddresses([FromQuery] string? city, [FromQuery] string? country)
        {
            var addressBussiness = new clsAddressBussiness(0, null, null, null, null, null, null);
            DataTable dt = addressBussiness.GetAllAdsresses(city, country);

            return Ok(dt);
        }

     
        [HttpPost ("CreateAddress")]
        public IActionResult AddAddress([FromBody] AddressCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // check the validations of input

            var addressBussiness = new clsAddressBussiness(0, null, null, null, null, null, null);
            bool success = addressBussiness.AddNewAddress(dto.UserID, dto.Street, dto.City, dto.State ?? string.Empty, dto.PostalCode ?? string.Empty, dto.Country);

            if (!success)
                return StatusCode(500, "Failed to add address.");

            return Ok("Address added successfully.");
        }

        [HttpPut("UpdateAddress{userId}")]
        public IActionResult UpdateAddress(int userId, [FromBody] AddressUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addressBussiness = new clsAddressBussiness(0, null, null, null, null, null, null);
            bool success = addressBussiness.UpdateAddress(userId, dto.Street, dto.City, dto.Country, dto.State, dto.PostalCode);

            if (!success)
                return NotFound("Address not found or update failed.");

            return Ok("Address updated successfully.");
        }

        [HttpDelete("DeleteAddress{userId}")]
        public IActionResult DeleteAddress(int userId)
        {
            var addressBussiness = new clsAddressBussiness(0, null, null, null, null, null, null);
            bool success = addressBussiness.DeleteAddress(userId);

            if (!success)
                return NotFound("Address not found.");

            return Ok("Address deleted successfully.");
        }
    }

    public class AddressCreateDTO
    {
        public int UserID { get; set; }
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = "";
    }

    public class AddressUpdateDTO
    {
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = "";
    }
}
