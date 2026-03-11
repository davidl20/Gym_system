using EvolCep.Repositories;
using EvolCep.Repositories.Interfaces;
using EvolCep.Services;
using EvolCep.Services.Interfaces;
using EvolCep.Services.WorkSessions;

namespace EvolCep.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<IWorkoutSessionCancellationService, WorkoutSessionCancellationService>();
            services.AddScoped<IWorkoutSessionCreationService, WorkoutSessionCreationService>();
            services.AddScoped<IWorkoutSessionEnrollmentService, WorkoutSessionEnrollmentService>();
            services.AddScoped<IWorkoutSessionQueryService, WorkoutSessionQueryService>();

            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IMembershipRepository, MembershipReppository>();

            //Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
