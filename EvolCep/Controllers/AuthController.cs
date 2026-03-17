using Microsoft.AspNetCore.Mvc;
using EvolCep.Shared.Dtos.Auth;
using EvolCep.Dtos.Auth;
using EvolCep.Services.Interfaces;

namespace EvolCep.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await _authService.RegisterClientAsync(dto);

            return StatusCode(201, new { message = "Usuario registrado exitosamente. Ya puedes iniciar sesión." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var response = await _authService.RefreshTokenAsync(dto);
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequestDto dto)
        {
            await _authService.LogoutAsync(dto.RefreshToken);

            return Ok(new
            {
                message = "Logout exitoso"
            });
        }
    }
}
