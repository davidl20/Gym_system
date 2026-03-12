using EvolCep.Models;

namespace EvolCep.Repositories.Interfaces
{
    public interface IClientMembershipRespository : IGenericRepository<ClientMembership>
    {
        Task<ClientMembership?> GetActiveMembershipAsync(int clientId);
        Task<IEnumerable<ClientMembership>> GetHistoryAsync(int clientId);
    }
}
