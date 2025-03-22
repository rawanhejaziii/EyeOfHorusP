using Microsoft.AspNetCore.Mvc;
using EyeOfHorusP.Application.DTOs.Auth;
using EyeOfHorusP.Application.Interfaces;

namespace EyeOfHorusP.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthUserController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var result = await _authService.RegisterAsync(registerRequestDTO);
            return Ok(result);
        }
    }
}
