using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

/** Clase que maneja las membresías compradas por los clientes**/

namespace EvolCep.Repositories
{
    public class ClientMembershipRepository : GenericRepository<ClientMembership>, IClientMembershipRepository
    {
        public ClientMembershipRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<ClientMembership?> GetActiveMembershipAsync(int clientId)
        {
            return await _dbSet
                .Include(cm => cm.MembershipPlan)
                .Where(cm =>
                    cm.ClientId == clientId &&
                    cm.EndDate >= DateTime.UtcNow &&
                    cm.RemainingClasses > 0)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClientMembership>> GetHistoryAsync(int clientId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(cm => cm.MembershipPlan)
                .Where(cm => cm.ClientId == clientId)
                .OrderByDescending(cm => cm.StartDate)
                .ToListAsync();
        }
    }
}
