using EvolCep.Data;
using EvolCep.Dtos;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Repositories
{
    public class WorkoutSessionRepository : GenericRepository<WorkoutSession>,IWorkoutSessionRepository
    {
        private readonly AppDbContext _context;

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
            var possibleOverlap = await _context.WorkoutSessions
                .Where(ws =>
                    ws.StartDateTime < end &&
                    ws.StartDateTime >= start.AddHours(-2))
                .ToListAsync();

            return possibleOverlap.Any(ws =>
            {
                var existingEnd = ws.StartDateTime.Add(ws.Duration);
                return ws.StartDateTime < end && existingEnd > start;
            });
        }

        public async Task<List<WorkoutSessionTodayDto>> GetTodaySessions(int clientid)
        {
            var now = DateTime.UtcNow;
            var startOfDay = now.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.WorkoutSessions
                .AsNoTracking()
                .Where(ws =>
                    ws.StartDateTime >= startOfDay &&
                    ws.StartDateTime < endOfDay &&
                    ws.StartDateTime > now
                    )
                .OrderBy(ws => ws.StartDateTime)
                .Select(ws => new WorkoutSessionTodayDto
                {
                    WorkoutSessionId = ws.Id,
                    StartDateTime = ws.StartDateTime,
                    EndDateTime = ws.StartDateTime.Add(ws.Duration),
                    AvailableSpots = ws.MaxClients - ws.ClientWorkoutSessions.Count(),
                    IsEnrolled = ws.ClientWorkoutSessions
                    .Any(cws => cws.ClientId == clientid)
                })
                .ToListAsync();
        }
    }
}
