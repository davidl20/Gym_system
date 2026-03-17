using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;


namespace EvolCep_Frontend.Client.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous;
        private readonly LocalStorageService _localStorageService;

        private const string AccesTokenKey = "auth_token";
        private const string RefreshTokenKey = "refresh_token";

        public ApiAuthenticationStateProvider(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string> (AccesTokenKey);

            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            if (jwt.ValidTo <= DateTime.UtcNow)
            {
                await ClearTokenAsync();
                return _anonymous;
            }

            return BuildAuthState(jwt);
        }

        public async Task MarkUserAsAuthenticated (string accesToken, string refreshToken)
        {
            await _localStorageService.SetItemAsync(AccesTokenKey, accesToken);
            await _localStorageService.SetItemAsync(RefreshTokenKey, refreshToken);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accesToken);

            NotifyAuthenticationStateChanged(
                Task.FromResult(BuildAuthState(jwt))
                );
        }

        public async Task MarkUserAsLoggedOut()
        {
            await ClearTokenAsync();

            NotifyAuthenticationStateChanged(
                Task.FromResult(_anonymous)
                );
        }

        public async Task<string?> GetAccesTokenAsync()
            => await _localStorageService.GetItemAsync<string>(AccesTokenKey);

        public async Task<string?> GetRefreshTokenAsync()
            => await _localStorageService.GetItemAsync<string>(RefreshTokenKey);

        private AuthenticationState BuildAuthState (JwtSecurityToken jwt)
        {
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task ClearTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(AccesTokenKey);
            await _localStorageService.RemoveItemAsync(RefreshTokenKey);
        }
    }
}
