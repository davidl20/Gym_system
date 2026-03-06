using EvolCep.Services;
using EvolCep.Services.Interfaces;

namespace EvolCep.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();

            return services;
        }
    }
}
