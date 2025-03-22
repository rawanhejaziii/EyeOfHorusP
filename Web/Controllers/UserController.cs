using EyeOfHorusP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeOfHorusP.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile() // جعل الميثود async
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            var user = await userService.GetUserById(userId); // استخدام await
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.Name // التأكد من أن FullName موجودة أو استبدالها بـ Name
            });
        }
    }
}
