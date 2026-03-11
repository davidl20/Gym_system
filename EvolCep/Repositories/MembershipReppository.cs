using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using System.Linq.Expressions;

namespace EvolCep.Repositories
{
    public class MembershipReppository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly AppDbContext _context;

        public MembershipReppository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddAsync(Membership entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Membership>> FindAsync(Expression<Func<Membership, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Remove(Membership entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Membership entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Membership>> IGenericRepository<Membership>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Membership?> IGenericRepository<Membership>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
