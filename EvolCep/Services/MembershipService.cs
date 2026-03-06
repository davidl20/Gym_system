using EvolCep.Data;
using EvolCep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly AppDbContext _context;

        public MembershipService(AppDbContext context)
        {
            _context = context;
        }
        public async Task BuyAsync(int clientId, int membershipId)
        {
            var client = await _context.Clients
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                throw new Exception("El cliente no existe");

            if (client.Membership != null &&
                client.Membership.EndDate >= DateTime.Now &&
                client.Membership.RemainingClasses > 0)
            {
                throw new Exception("El cliente ya tiene una membresía activa");
            }

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.Id == membershipId);

            if (membership == null)
                throw new Exception("La membresía no existe");

            // Activar membresía
            membership.StartDate = DateTime.Now;
            membership.EndDate = DateTime.Now.AddMonths(1);
            membership.RemainingClasses = membership.TotalClasses;

            client.Membership = membership;

            await _context.SaveChangesAsync();
        }
    }
}
