using EvolCep.Dtos;

namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionCreationService
    {
        Task CreateAsync (CreateWorkoutSessionDto dto);
    }
}
