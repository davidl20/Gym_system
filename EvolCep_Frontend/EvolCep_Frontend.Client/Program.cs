using EvolCep_Frontend.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//1. Servicios Base
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>());

//2.Interceptor
builder.Services.AddTransient<AuthMessageHandler>();

//3.HttpClient normal (el que usara la app y pasará por el interceptor)
builder.Services.AddHttpClient("EvolCepAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7118");
}).AddHttpMessageHandler<AuthMessageHandler>();

//Cliente por defecto para la app
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("EvolCepAPI"));

// 4. El HttpClient especial para el AuthService (SIN INTERCEPTOR, para que pueda hacer login/refresh sin interrumpirse)
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});

await builder.Build().RunAsync();
