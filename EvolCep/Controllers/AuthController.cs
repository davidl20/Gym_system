using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register (RegisterDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.RegisterClientAsync(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            
        }

        [HttpPost ("refresh")]
        public async Task<IActionResult> Refresh (RefreshTokenRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.RefreshTokenAsync(dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized (ex.Message);
            }
               
        }
    }
}
