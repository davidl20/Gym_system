using EvolCep.Dtos;

namespace EvolCep.Services.Interfaces
{
    public interface IWorkoutSessionService
    {
        Task EnrollAsync(int clientId, int workoutSessionId);
        Task CancelAsync (int clientId, int workoutSessionId);
        Task<IEnumerable<WorkoutSessionTodayDto>> GetTodaySessionsAsync(int clientId);
        Task CreateAsync (CreateWorkoutSessionDto dto);
    }
}
