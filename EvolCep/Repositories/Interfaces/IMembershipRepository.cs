using EvolCep.Models;
namespace EvolCep.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        Task<IEnumerable<Membership>> GetAvailableAsync();
    }
}
