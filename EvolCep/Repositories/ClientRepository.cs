using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Repositories
{
    public class ClientRepository : GenericRepository<Client>,IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context) : base(context)
        {
        }
        public async Task AddEnrollmentAsync(ClientWorkoutSession enrollment)
        {
            await _context.ClientWorkoutSessions.AddAsync(enrollment);
        }

        public async Task<bool> ClientAlreadyEnrolledAsync(int clientId, int workoutSessionId)
        {
            return await _context.ClientWorkoutSessions
                .AnyAsync(cws =>
                    cws.ClientId == clientId &&
                    cws.WorkoutSessionId == workoutSessionId);
        }

        public async Task<bool> ClientHasClassThatDayAsync(int clientId, DateTime date)
        {
            return await _context.ClientWorkoutSessions
                .AnyAsync(cws =>
                    cws.ClientId == clientId &&
                    cws.StartDateTime.Date == date.Date);
        }

        public async Task<Client?> GetClientWithMembershipAsync(int clientId)
        {
            return await _context.Clients.
                Include(c => c.Membership).
                FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<ClientWorkoutSession?> GetEnrollmentAsync(int clientId, int workoutSessionId)
        {
            return await _context.ClientWorkoutSessions
                .FirstOrDefaultAsync(cws =>
                    cws.ClientId == clientId &&
                    cws.WorkoutSessionId == workoutSessionId);
        }

        public async Task RemoveEnrollmentAsync(ClientWorkoutSession enrollment)
        {
            _context.ClientWorkoutSessions.Remove(enrollment);
            await Task.CompletedTask;
        }
    }
}
