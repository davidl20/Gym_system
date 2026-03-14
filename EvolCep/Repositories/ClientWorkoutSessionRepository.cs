using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

//Esta clase maneja las inscripciones a clases
namespace EvolCep.Repositories
{
    public class ClientWorkoutSessionRepository : GenericRepository<ClientWorkoutSession>, IClientWorkoutSessionRepository
    {
        public ClientWorkoutSessionRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<bool> ExistsAsync(int clientId, int workoutSessionId)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(cws =>
                    cws.ClientId == clientId &&
                    cws.WorkoutSessionId == workoutSessionId);
        }

        public async Task<ClientWorkoutSession?> GetClientSessionAsync(int clientId, int workoutSessionId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x =>
                    x.ClientId == clientId &&
                    x.WorkoutSessionId == workoutSessionId);
        }

        public async Task<IEnumerable<ClientWorkoutSession>> GetUpcomingSessionsByClientAsync(int clientId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(cws => cws.WorkoutSession)
                    .ThenInclude(ws => ws.Description)
                .Where(cws => cws.ClientId == clientId && 
                       cws.WorkoutSession.StartDateTime >= DateTime.UtcNow)
                .OrderBy(cws => cws.WorkoutSession.StartDateTime)
                .ToListAsync();
        }

        public async Task<bool> HasClassThatDayAsync(int clientId, DateTime date)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(cws =>
                     cws.ClientId == clientId &&
                     cws.StartDateTime.Date == date.Date);
        }
    }
}
