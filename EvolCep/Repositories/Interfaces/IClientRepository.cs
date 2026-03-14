using EvolCep.Models;

namespace EvolCep.Repositories.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetByApplicationUserIdAsync (string clientId);
        Task<Client?> GetClientWithMemberShipAsync(int clientId);
    }
}
