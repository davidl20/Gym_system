using EvolCep.Models;
using EvolCep.Shared.Dtos.Sessions;

namespace EvolCep.Repositories.Interfaces
{
    public interface IWorkoutSessionRepository : IGenericRepository<WorkoutSession>
    {
        Task<int> CountEnrollmentsAsync(int workoutSessionId);
        Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsByDateAsync(int clientid, DateTime date);
        Task <bool> HasOverlapAsync(DateTime start, DateTime end);
    }
}
