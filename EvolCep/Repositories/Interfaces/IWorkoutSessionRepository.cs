using EvolCep.Models;
using EvolCep.Dtos;

namespace EvolCep.Repositories.Interfaces
{
    public interface IWorkoutSessionRepository : IGenericRepository<WorkoutSession>
    {
        Task<int> CountEnrollmentsAsync(int workoutSessionId);
        Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsByDateAsync(int clientid, DateTime date);
        Task <bool> HasOverlapAsync(DateTime start, DateTime end);
    }
}
