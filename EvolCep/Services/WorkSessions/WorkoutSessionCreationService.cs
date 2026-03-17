using EvolCep.Shared.Dtos.Sessions;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services.WorkSessions
{
    public class WorkoutSessionCreationService : IWorkoutSessionCreationService
    {
        private readonly IWorkoutSessionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutSessionCreationService(
            IWorkoutSessionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CreateWorkoutSessionDto dto)
        {
            var start = dto.StartDateTime;

            if (start <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("No se puede crear clases en horas pasadas");
            }

            var end = dto.StartDateTime.Add(dto.Duration);

            var overlapExist = await _repository.HasOverlapAsync(start, end);

            if (overlapExist)
                throw new InvalidOperationException("Ya existe una clase programada en ese horario");

            var session = new WorkoutSession
            {
                Description = dto.Description,
                StartDateTime = start,
                Duration = dto.Duration,
                MaxClients = dto.MaxClients ?? 10
            };

            await _repository.AddAsync(session);

            await _unitOfWork .SaveChangesAsync();
        }
    }
}
