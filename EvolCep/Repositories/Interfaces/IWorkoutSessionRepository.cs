using EvolCep.Models;
using EvolCep.Dtos;

namespace EvolCep.Repositories.Interfaces
{
    public interface IWorkoutSessionRepository : IGenericRepository<WorkoutSession>
    {
        Task<int> CountEnrollmentsAsync(int workoutSessionId);
        Task<List<WorkoutSessionTodayDto>> GetTodaySessions(int clientid);
        Task <bool> HasOverlapAsync(DateTime start, DateTime end);
    }
}
