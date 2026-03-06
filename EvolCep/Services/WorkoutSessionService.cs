using EvolCep.Data;
using EvolCep.Dtos;
using EvolCep.Models;
using EvolCep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Services
{
    public class WorkoutSessionService : IWorkoutSessionService
    {
        //private const int MAX_CLIENTS_PER_SESSION = 10;

        private readonly AppDbContext _context;

        public WorkoutSessionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CancelAsync(int clientId, int workoutSessionId)
        {
            var enrollment = await _context.ClientWorkoutSessions
                .FirstOrDefaultAsync (x =>
                x.ClientId == clientId &&
                x.WorkoutSessionId == workoutSessionId);

            if (enrollment == null)
                throw new Exception("No tiene clases agendadas");

            var cancellationDeadLine =  enrollment.StartDateTime.AddHours(-2);

            if (DateTime.Now > cancellationDeadLine)
                throw new Exception("La clase solo puede ser cancelada con minimo 2 horas de anticipación");

            var client = await _context.Clients
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(C => C.Id == clientId);

            if (client?.Membership != null)
            {
                client.Membership.RemainingClasses++;
            }

            _context.ClientWorkoutSessions.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateWorkoutSessionDto dto)
        {
            var start = dto.StartDateTime;

            if (start <= DateTime.Now)
                throw new Exception("No se pueden crear clases en el pasado");

            var duration = dto.Duration ?? TimeSpan.FromHours(1);
            var end = start.Add(duration);

            //Traer solo clases cercanas
            var possibleOverlaps = await _context.WorkoutSessions
                .Where(ws =>
                    ws.StartDateTime < end &&
                    ws.StartDateTime >= start.AddHours(-2)) // ventana segura
                .ToListAsync();

            //Validar solapamiento en memoria
            var overlapExist = possibleOverlaps.Any(ws =>
            {
                var existingEnd = ws.StartDateTime.Add(ws.Duration);
                return ws.StartDateTime < end && existingEnd > start;
            });

            if (overlapExist)
                throw new Exception("Ya existe una clase en ese rango de horario");

            var session = new WorkoutSession
            {
                StartDateTime = start,
                Duration = duration,
                MaxClients = dto.MaxClients ?? 10
            };

            _context.Add (session);
            await _context.SaveChangesAsync();
        }

        public async Task EnrollAsync(int clientId, int workoutSessionId)
        {
            var now = DateTime.Now;

            var clientExist = await _context.Clients
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(c => c.Id == clientId);
                
            if (clientExist == null)
                throw new Exception("El cliente no existe");

            var membership = clientExist.Membership;

            if (membership == null)
                throw new Exception("El cliente no cuenta con una membresía activa");

            if (membership.RemainingClasses <= 0)
                throw new Exception("No tiene clases disponibles");

            var session = await _context.WorkoutSessions
                .FirstOrDefaultAsync(ws => ws.Id == workoutSessionId);

            if (session == null)
                throw new Exception("La clase no existe");

            var sessionDate = session.StartDateTime.Date;

            var alreadyHasClassThatDay = await _context.ClientWorkoutSessions
                .AnyAsync(cws =>
                cws.ClientId == clientId &&
                cws.StartDateTime.Date == sessionDate
                );

            if (alreadyHasClassThatDay)
                throw new Exception("Solo puedes tomar una clase por día");

            //Evitar doble inscripción
            var alreadyEnrolled = await _context.ClientWorkoutSessions
                .AnyAsync(cws =>
                cws.ClientId == clientId &&
                cws.WorkoutSessionId == workoutSessionId);

            if (alreadyEnrolled)
                throw new Exception("El cliente ya tiene una clase en esta fecha");

            var count = await _context.ClientWorkoutSessions
            .CountAsync(cws => cws.WorkoutSessionId == workoutSessionId);

            if (count >= session.MaxClients)
                throw new Exception("La clase ya alcanzó el máximo de clientes");

            var enrollment = new ClientWorkoutSession
            {
                ClientId = clientId,
                WorkoutSessionId = workoutSessionId,
                StartDateTime = session.StartDateTime
            };

            _context.ClientWorkoutSessions.Add(enrollment);

            membership.RemainingClasses--;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkoutSessionTodayDto>> GetTodaySessionsAsync(int clientId)
        {
            var now = DateTime.Now;
            var today = DateTime.Today;

            return await _context.WorkoutSessions
                .Where (ws => ws
                .StartDateTime.Date == today && 
                 ws.StartDateTime > now
                 )
                .Select (ws => new WorkoutSessionTodayDto
                {
                    WorkoutSessionId = ws.Id,
                    StartDateTime = ws.StartDateTime,
                    EndDateTime = ws.StartDateTime.Add(ws.Duration),
                    AvailableSpots = ws.MaxClients - ws.ClientWorkoutSessions.Count
                })
                .OrderBy (ws => ws.StartDateTime)
                .ToListAsync();
        }
    }
}
