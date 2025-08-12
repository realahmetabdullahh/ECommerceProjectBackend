using E_Commerce.Bussiness;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_Commerce.API.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = clsUsersBussiness.ReadUserInfoByUsernameAndPassword(request.Username, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            return Ok(new
            {
                user.UserID,
                user.UserName,
                user.Email,
                user.Role,
                user.CreatedAt
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (clsUsersBussiness.IsUserExists(request.Username))
                return BadRequest(new { message = "Username already exists." });

            bool success = clsUsersBussiness.AddNewUser(request.Username, request.Email, request.Password, request.Role);
            if (!success)
                return StatusCode(500, new { message = "Failed to create user." });

            return Ok(new { message = "User registered successfully." });
        }

        [HttpGet("Get{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = clsUsersBussiness.ReadUserInfoByUserID(id);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.UserID,
                user.UserName,
                user.Email,
                user.Role,
                user.CreatedAt
            });
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers(string? Roll = null)
        {
            DataTable dt = clsUsersBussiness.GetAllUsers(Roll);

            var result = DataTableToList(dt);

            return Ok(result); 
        }

         List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }

            return list;
        }
        [HttpPut("UpdateUser{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            bool exists = clsUsersBussiness.IsUserExists(id);
            if (!exists) return NotFound();

            bool updated = clsUsersBussiness.UpdateUser(id, request.Username, request.Email, request.Role);
            if (!updated) return StatusCode(500, new { message = "Failed to update user." });

            return Ok(new { message = "User updated successfully." });
        }

       
        [HttpDelete("DeleteUser{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            bool exists = clsUsersBussiness.IsUserExists(id);
            if (!exists) return NotFound();

            bool deleted = clsUsersBussiness.DeleteUser(id);
            if (!deleted) return StatusCode(500, new { message = "Failed to delete user." });

            return Ok(new { message = "User deleted successfully." });
        }
        
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Member";  // Default role
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Role { get; set; }  // Optional
    }
}
