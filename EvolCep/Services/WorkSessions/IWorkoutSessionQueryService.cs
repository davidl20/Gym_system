using EvolCep.Dtos;

namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionQueryService
    {
        Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsTodayAsync(int clientid);
    }
}
