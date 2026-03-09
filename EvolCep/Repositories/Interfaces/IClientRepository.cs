using EvolCep.Models;

namespace EvolCep.Repositories.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetClientWithMembershipAsync(int clientId);
        Task<bool> ClientHasClassThatDayAsync(int clientId, DateTime date);
        Task<bool> ClientAlreadyEnrolledAsync(int clientId, int workoutSessionId);
        Task AddEnrollmentAsync(ClientWorkoutSession enrollment);
        Task <ClientWorkoutSession?> GetEnrollmentAsync(int clientId, int workoutSessionId);
        Task RemoveEnrollmentAsync(ClientWorkoutSession enrollment);
    }
}
