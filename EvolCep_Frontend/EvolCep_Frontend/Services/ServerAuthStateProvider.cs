using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EvolCep_Frontend.Services
{
    public class ServerAuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }
    }
}
