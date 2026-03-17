using EvolCep.Shared.Dtos.Sessions;

namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionQueryService
    {
        Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsByDateAsync(int clientId, DateTime date);
    }
}
