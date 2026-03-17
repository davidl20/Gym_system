using System.Net;
using System.Net.Http.Headers;

namespace EvolCep_Frontend.Client.Services
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly ApiAuthenticationStateProvider _authProvider;
        private readonly IServiceProvider _serviceProvider;

        private static readonly SemaphoreSlim _refreshLock = new SemaphoreSlim (1, 1);

        public AuthMessageHandler (ApiAuthenticationStateProvider authProvider, IServiceProvider serviceProvider)
        {
            _authProvider = authProvider;
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync (
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var token = await _authProvider.GetAccesTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _refreshLock.WaitAsync(cancellationToken);

                try
                {
                    var authService = _serviceProvider.GetRequiredService<AuthService>();

                    var refreshed = await authService.TryRefreshTokenAsync();

                    if (!refreshed)
                    {
                        await _authProvider.MarkUserAsLoggedOut();
                        return response;
                    }

                    var newToken = await _authProvider.GetAccesTokenAsync();

                    var clonedRequest = await CloneRequestAsync(request);

                    clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                    return await base.SendAsync(clonedRequest, cancellationToken);
                }
                finally
                {
                    _refreshLock.Release(); 
                }
            }

            return response;
        }

        private async Task<HttpRequestMessage> CloneRequestAsync (HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            if (request.Content != null)
            {
                var bytes = await request.Content.ReadAsByteArrayAsync();

                clone.Content = new ByteArrayContent(bytes);

                if (request.Content.Headers != null)
                {
                    foreach (var h in request.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
                }
            }

            clone.Version = request.Version;

            foreach (var prop in request.Options)
                clone.Options.Set(new HttpRequestOptionsKey<object?>(prop.Key), prop.Value);

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}
