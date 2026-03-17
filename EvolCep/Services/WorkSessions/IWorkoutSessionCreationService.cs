using EvolCep.Shared.Dtos.Sessions;

namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionCreationService
    {
        Task CreateAsync (CreateWorkoutSessionDto dto);
    }
}
