using EvolCep.Models;
using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services.WorkSessions
{
    public class WorkoutSessionEnrollmentService : IWorkoutSessionEnrollmentService
    {
        private readonly IWorkoutSessionRepository _repository;
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutSessionEnrollmentService(
            IWorkoutSessionRepository repository,
            IClientRepository clientRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task EnrollAsync(int clientId, int workoutSessionId)
        {
            var client = await _clientRepository.GetClientWithMembershipAsync(clientId) ?? throw new Exception("El cliente no existe");

            var membership = client.Membership ?? throw new Exception("El cliente no tiene una membresía activa");

            if (membership.RemainingClasses <= 0)
                throw new Exception("No tiene clases disponibles");

            var session = await _repository.GetByIdAsync(workoutSessionId) ?? throw new Exception("La sesión de entrenamiento no existe");

            var sessionDate = session.StartDateTime.Date;

            var alreadyHasClassThatDay = await _clientRepository.ClientHasClassThatDayAsync(clientId, sessionDate);

            if (alreadyHasClassThatDay)
                throw new Exception("Solo puedes tomar una clase por día");

            var alreadyEnrolled = await _clientRepository.ClientAlreadyEnrolledAsync(clientId, workoutSessionId);
                
            if (alreadyEnrolled)
                throw new Exception("El cliente ya está inscrito en esta clase");

            var count = await _repository.CountEnrollmentsAsync(workoutSessionId);

            if (count >= session.MaxClients)
                throw new Exception("La clase ya alcanzó el máximo de clientes");

            var enrollment = new ClientWorkoutSession
            {
                ClientId = clientId,
                WorkoutSessionId = workoutSessionId,
                StartDateTime = session.StartDateTime
            };

            await _clientRepository.AddEnrollmentAsync(enrollment);

            membership.RemainingClasses --;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
