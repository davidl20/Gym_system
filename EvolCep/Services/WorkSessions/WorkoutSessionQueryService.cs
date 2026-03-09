using EvolCep.Data;
using EvolCep.Dtos;
using Microsoft.EntityFrameworkCore;
using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services.WorkSessions
{
    public class WorkoutSessionQueryService : IWorkoutSessionQueryService
    {
        private readonly IWorkoutSessionRepository _repository;

        public WorkoutSessionQueryService(IWorkoutSessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsTodayAsync(int clientid)
        {
            return await _repository.GetTodaySessions(clientid);
        }
    }
}

