using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvolCep_Frontend.Client.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;

        private const string TOKEN_KEY = "authToken";

        public ApiAuthenticationStateProvider(HttpClient httpClient, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorageService.GetItemAsync(TOKEN_KEY);

            if (string.IsNullOrWhiteSpace(savedToken))
                return CreateAnonymousState();

            SetAuthorizationHeader(savedToken);

            var claims = ParseClaimsFromJwt(savedToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated (string token)
        {
            await _localStorageService.SetItemAsync(TOKEN_KEY, token);

            SetAuthorizationHeader(token);

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")
                );

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user))
                );
        }

        public async Task MarkUserAsLoggedOut(string token)
        {
            await _localStorageService.RemoveItemAsync(TOKEN_KEY);

            _httpClient.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(
                Task.FromResult(CreateAnonymousState())
            );
        }

        private void SetAuthorizationHeader (string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private AuthenticationState CreateAnonymousState()
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity())
            );
        }
        private IEnumerable<Claim> ParseClaimsFromJwt (string jwt)
        {
            var claims = new List<Claim>();

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer
                .Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null)
                return claims;

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value is JsonElement element &&
                    element.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in element.EnumerateArray())
                    {
                        claims.Add(new Claim(kvp.Key, item.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
                }
            }

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
