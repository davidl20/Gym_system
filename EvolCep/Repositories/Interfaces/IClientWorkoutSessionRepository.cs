using EvolCep.Models;

namespace EvolCep.Repositories.Interfaces
{
    public interface IClientWorkoutSessionRepository : IGenericRepository<ClientWorkoutSession>
    {
        Task<bool> ExistsAsync (int clientId, int workoutSessionId);
        Task<bool> HasClassThatDayAsync (int clientId, DateTime date);
        Task<ClientWorkoutSession?> GetClientSessionAsync (int clientId, int workoutSessionId);
        Task<IEnumerable<ClientWorkoutSession>> GetUpcomingSessionsByClientAsync (int clientId);
    }
}
