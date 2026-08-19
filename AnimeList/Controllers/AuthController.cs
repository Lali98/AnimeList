using AnimeList.Dtos.Auth;
using AnimeList.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest,
                    message = result.Error
                });
            }

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Sikeres regisztráció!"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result is null)
                return Unauthorized(new
                {
                    status = StatusCodes.Status401Unauthorized,
                    message = "Hibás felhasználónév vagy jelszó!"
                });
            return Ok(result);
        }
    }
}
