using EvolCep.Dtos.Auth;
using EvolCep.Shared.Dtos.Auth;


namespace EvolCep.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterClientAsync (RegisterDto dto);
        Task<AuthResponseDto> LoginAsync (LoginDto dto); 
        Task<AuthResponseDto> RefreshTokenAsync (RefreshTokenRequestDto dto);
        Task LogoutAsync(string refreshToken);
    }
}
