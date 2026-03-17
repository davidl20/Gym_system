using EvolCep.Constants;
using EvolCep.Data;
using EvolCep.Extensions;
using EvolCep.Middleware;
using EvolCep.Models;
using EvolCep.Services;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7022") // ?? PUERTO DEL FRONT
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//JWT Authentication (extesión)
builder.Services.AddJwtAuthentication(builder.Configuration);

//Authorization (policies)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientOnly", policy =>
        policy.RequireRole(Roles.Client));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(Roles.Admin));
});

//App Services
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("BlazorPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//Seed
await app.SeedRolesAsync();
await app.SeedAdminAsync();

app.Run();
