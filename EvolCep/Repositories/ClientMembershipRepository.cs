using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Repositories
{
    public class ClientMembershipRepository : GenericRepository<ClientMembership>, IClientMembershipRespository
    {
        private readonly AppDbContext _context;

        public ClientMembershipRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ClientMembership?> GetActiveMembershipAsync(int clientId)
        {
            return await _context.ClientMemberships
                .Include(m => m.MembershipPlan)
                .FirstOrDefaultAsync(m =>
                    m.ClientId == clientId &&
                    m.EndDate >= DateTime.UtcNow &&
                    m.RemainingClasses > 0);
        }

        public async Task<IEnumerable<ClientMembership>> GetHistoryAsync(int clientId)
        {
            return await _context.ClientMemberships
                .Include(m => m.MembershipPlan)
                .Where(m => m.ClientId == clientId)
                .OrderByDescending(m => m.StartDate)
                .ToListAsync();
        }
    }
}
