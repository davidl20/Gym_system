using EvolCep.Dtos;

namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionQueryService
    {
        Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsByDateAsync(int clientId, DateTime date);
    }
}
