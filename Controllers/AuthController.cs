using Microsoft.AspNetCore.Mvc;
using WeatherDashboardAPI.DTOs.Auth;
using WeatherDashboardAPI.Services;

namespace WeatherDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, user) = await _authService.RegisterAsync(dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new
            {
                message,
                user
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, response) = await _authService.LoginAsync(dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new
            {
                message,
                data = response
            });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
                return Unauthorized(new { message = "Token could not found." });

            var token = authHeader.Replace("Bearer ", "");

            var jwtHelper = HttpContext.RequestServices.GetRequiredService<Helpers.JwtHelper>();
            var userId = jwtHelper.ValidateToken(token);

            if (userId == null)
                return Unauthorized(new { message = "Invalid token." });

            var user = await _authService.GetUserByIdAsync(userId.Value);

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }
    }
}