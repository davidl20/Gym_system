using EvolCep.Data;
using EvolCep.Dtos;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                throw new Exception("No se puede crear clases en horas pasadas");
            }

            var duration = dto.Duration ?? TimeSpan.FromHours(1);
            var end = start.Add(duration);

            var overlapExist = await _repository.HasOverlapAsync(start, end);

            if (overlapExist)
                throw new Exception("Ya existe una clase programada en ese horario o en un horario cercano. Por favor, elige otro horario.");

            var session = new WorkoutSession
            {
                StartDateTime = start,
                Duration = duration,
                MaxClients = dto.MaxClients ?? 10
            };

            await _repository.AddAsync(session);

            await _unitOfWork .SaveChangesAsync();
        }
    }
}
