using EvolCep_Frontend.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. ESTADO DE AUTENTICACIÓN
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ApiAuthenticationStateProvider>());

//2.SERVICIOS DE INFRAESTRUCTURA
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddTransient<AuthMessageHandler>();


// 3.CONFIGURACIÓN DE HTTP CLIENTS 
//Especializadopara Auth. Se usa para Login/Register para evitar bucles infinitos
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});

// 3. CLIENTE GENERAL CON INTERCEPTOR
//Se usa para todas las llamadas que requieren el token JWT
builder.Services.AddHttpClient("EvolCepAPI", client => 
{
    client.BaseAddress = new Uri("https://localhost:7118/");
})
    .AddHttpMessageHandler<AuthMessageHandler>();
    

await builder.Build().RunAsync();