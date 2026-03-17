using EvolCep.Shared.Dtos.Sessions;
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

        public async Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsByDateAsync(int clientId, DateTime date)
        {
            if (date.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException("No se pueden consultar clases en fechas pasadas");
            }
            var sessions = await _repository.GetSessionsByDateAsync(clientId, date.Date);

            var threshold = DateTime.UtcNow.AddHours(2);

            if (date.Date == DateTime.UtcNow.Date)
            {
                sessions = sessions.Where (s => s.StartDateTime >= threshold);
            }

            return sessions.OrderBy(s => s.StartDateTime);
        }
    }
}

