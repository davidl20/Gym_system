using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace EvolCep_Frontend.Client.Services
{
    public class ApiAuthenticationStateProvider (LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        //private readonly LocalStorageService _localStorageService;
        private const string AccesTokenKey = "auth_token";
        private const string RefreshTokenKey = "refresh_token";

        /*public ApiAuthenticationStateProvider(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }*/

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await GetAccesTokenAsync();

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
            catch
            {
                return _anonymous;
            }
        }

        public async Task MarkUserAsAuthenticated (string accesToken, string refreshToken)
        {
            await localStorageService.SetItemAsync(AccesTokenKey, accesToken);
            await localStorageService.SetItemAsync(RefreshTokenKey, refreshToken);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accesToken);
            var authState = BuildAuthState(jwt);

            NotifyAuthenticationStateChanged(
                Task.FromResult(authState)
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
            => await localStorageService.GetItemAsync<string>(AccesTokenKey);

        public async Task<string?> GetRefreshTokenAsync()
            => await localStorageService.GetItemAsync<string>(RefreshTokenKey);

        private AuthenticationState BuildAuthState (JwtSecurityToken jwt)
        {
            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            var identity = new ClaimsIdentity(
                jwt.Claims,
                "jwt",
                "name",
                roleClaim?.Type ?? "role" // Usamos el tipo de claim que realmente encontramos
            );

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task ClearTokenAsync()
        {
            await localStorageService.RemoveItemAsync(AccesTokenKey);
            await localStorageService.RemoveItemAsync(RefreshTokenKey);
        }
    }
}
