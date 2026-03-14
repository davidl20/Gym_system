using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Client?> GetByApplicationUserIdAsync(string clientId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.ApplicationUserId == clientId);
        }

        public async Task<Client?> GetClientWithMemberShipAsync(int clientId)
        {
            return await _dbSet
                .Include(c => c.Memberships)
                    .ThenInclude(cm => cm.MembershipPlan)
                .FirstOrDefaultAsync (c => c.Id == clientId);
        }
    }
}
