using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EvolCep.Repositories
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Membership>> GetAvailableAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
