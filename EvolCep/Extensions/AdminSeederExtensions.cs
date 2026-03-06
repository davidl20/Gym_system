using EvolCep.Models;
using EvolCep.Constants;
using Microsoft.AspNetCore.Identity;

namespace EvolCep.Extensions
{
    public static class AdminSeederExtensions
    {
        public static async Task SeedAdminAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>> ();
            
            var configuration = scope.ServiceProvider
                .GetRequiredService<IConfiguration> ();

            var email = configuration["AdminSeed:Email"];
            var password = configuration["AdminSeed:Password"];
            var phone = configuration["AdminSeed:PhoneNumber"];

            var adminUser = await userManager.FindByEmailAsync(email!);

            if (adminUser != null)
                return;

            adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phone,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, password!);

            if (!result.Succeeded)
                throw new Exception(
                    "Error creando admin: " +
                    string.Join(", ", result.Errors.Select(e => e.Description))
                    );

            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
    }
}
