using EvolCep.Data;
using EvolCep.Dtos;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

//Esta clase maneja las clases del gimnasio
namespace EvolCep.Repositories
{
    public class WorkoutSessionRepository : GenericRepository<WorkoutSession>,IWorkoutSessionRepository
    {
        public WorkoutSessionRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<int> CountEnrollmentsAsync(int workoutSessionId)
        {
            return await _context.ClientWorkoutSessions
                .CountAsync(cws => cws.WorkoutSessionId == workoutSessionId);
        }

        public async Task<bool> HasOverlapAsync (DateTime start, DateTime end)
        {
            return await _dbSet
                .AnyAsync(ws =>
                    ws.StartDateTime < end &&
                    ws.StartDateTime.Add(ws.Duration) > start);
        }

        public async Task<IEnumerable<WorkoutSessionTodayDto>> GetSessionsByDateAsync(int clientid, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            return await _context.WorkoutSessions
                .AsNoTracking()
                .Where (s => s.StartDateTime >= startOfDay && s.StartDateTime < endOfDay)
                .Select (s => new WorkoutSessionTodayDto
                {
                    WorkoutSessionId = s.Id,
                    Description = s.Description,
                    StartDateTime = s.StartDateTime,
                    EndDateTime = s.StartDateTime.Add(s.Duration),
                    IsEnrolled = s.ClientWorkoutSessions.Any (cw => cw.ClientId == clientid)
                })
                .OrderBy (S => S.StartDateTime)
                .ToListAsync();
                
        }
    }
}
