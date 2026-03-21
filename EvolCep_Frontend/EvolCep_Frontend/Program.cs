using EvolCep_Frontend.Components;
using EvolCep_Frontend.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using EvolCep_Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// 1.SERVICIOS DE BLAZOR
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

//2.AUTENTICACIÓN Y ESTADO
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ApiAuthenticationStateProvider>());


//3.SERVICIOS DE APLICACIÓN
builder.Services.AddTransient<AuthMessageHandler>();
builder.Services.AddScoped<LocalStorageService>();

//HttpClient para el AuthService en el servidor
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7118/");
});

//HttpClient genérico por si algún otro componente lo pide
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7118/")
});

var app = builder.Build();

// PIPELINE DE CONFIGURACIÓN
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

//MAPEO DE COMPONENTES Y ENSAMBLADOS
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(EvolCep_Frontend.Client.Routes).Assembly);

app.Run();
