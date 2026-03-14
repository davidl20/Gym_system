using EvolCep.Models;
using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services.WorkSessions
{
    public class WorkoutSessionEnrollmentService : IWorkoutSessionEnrollmentService
    {
        private readonly IWorkoutSessionRepository _sessionrepository;
        private readonly IClientMembershipRepository _membershipRepository;
        private readonly IClientWorkoutSessionRepository _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutSessionEnrollmentService(
            IWorkoutSessionRepository workoutSessionRepository,
            IClientMembershipRepository membershipRepository,
            IClientWorkoutSessionRepository enrollmentRepository,
            IUnitOfWork unitOfWork)
        {
            _sessionrepository = workoutSessionRepository;
            _membershipRepository = membershipRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task EnrollAsync(int clientId, int workoutSessionId)
        {
            var activeMembership = await _membershipRepository.GetActiveMembershipAsync(clientId)
                ?? throw new InvalidOperationException("El cliente no tiene una membresía activa");

            if(activeMembership.RemainingClasses <= 0)
                throw new InvalidOperationException("No tiene clases disponibles");

            var session = await _sessionrepository.GetByIdAsync(workoutSessionId) 
                ?? throw new KeyNotFoundException("La sesión de entrenamiento no existe");

            var currentEnrollments = await _sessionrepository.CountEnrollmentsAsync(workoutSessionId);
            if (currentEnrollments >= session.MaxClients)
                throw new InvalidOperationException("Lo sentimos, la clase no cuenta con cupos disponibles");
             
            var alredyEnrolled = await _enrollmentRepository.ExistsAsync(clientId, workoutSessionId);
            if(alredyEnrolled)
                throw new InvalidOperationException("Ya estas inscrito en esta sesión");

            var hasClassToday = await _enrollmentRepository.HasClassThatDayAsync(clientId, session.StartDateTime);
            if(hasClassToday)
                throw new InvalidOperationException("Solo puedes tomar una clase por día");
            
            var enrollment = new ClientWorkoutSession
            {
                ClientId = clientId,
                WorkoutSessionId = workoutSessionId,
                StartDateTime = session.StartDateTime
            };

            await _enrollmentRepository.AddAsync(enrollment);

            activeMembership.RemainingClasses --;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
