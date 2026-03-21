using EvolCep.Shared.Dtos.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace EvolCep_Frontend.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ApiAuthenticationStateProvider _authProvider;

        public AuthService(HttpClient http, ApiAuthenticationStateProvider authProvider)
        {
            _http = http;
            _authProvider = authProvider;
        }

        public async Task<string> GetUserRoleAsync()
        {
            var token = await _authProvider.GetAccesTokenAsync();
            if (string.IsNullOrEmpty(token))
                return "Guest";

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == "role")
                 ?? jwt.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            var roleValue = roleClaim?.Value ?? "Customer";

            Console.WriteLine($"Rol detectado en el token: {roleValue}");

            return roleValue;
        }
        public async Task<bool> LoginAsync(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (result == null)
                return false;

            await _authProvider.MarkUserAsAuthenticated(result.AccessToken, result.RefreshToken);

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            var refreshToken = await _authProvider.GetRefreshTokenAsync();

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _http.PostAsJsonAsync("api/auth/logout", new
                {
                    refreshToken
                });
            }

            await _authProvider.MarkUserAsLoggedOut();
        }

        public async Task<bool> TryRefreshTokenAsync()
        {
            var refreshToken = await _authProvider.GetRefreshTokenAsync();

            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var response = await _http.PostAsJsonAsync("api/auth/refresh", new
            {
                refreshToken
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (result == null)
                return false;

            await _authProvider.MarkUserAsAuthenticated(
                result.AccessToken,
                result.RefreshToken);

            return true;
        }
    }
}
