using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services.WorkSessions
{
    public class WorkoutSessionCancellationService : IWorkoutSessionCancellationService
    {
        private readonly IClientWorkoutSessionRepository _enrollmentRepository;
        private readonly IClientMembershipRepository _clientMembershipRepository;
        private readonly IUnitOfWork _unitOfWork;   

        public WorkoutSessionCancellationService( 
            IClientWorkoutSessionRepository enrollmentRepository,
            IClientMembershipRepository clientMembershipRepository,
            IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _clientMembershipRepository = clientMembershipRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task CancelSessionAsync(int sessionId, int clientId)
        {
            var enrollment = await _enrollmentRepository.GetClientSessionAsync(sessionId, clientId)
                ?? throw new KeyNotFoundException("No se encontró una reserva para esta clase");

            var cancellationDeadLine = enrollment.StartDateTime.AddHours(-2);

            if (DateTime.UtcNow > cancellationDeadLine)
                throw new InvalidOperationException("La clase solo puede ser cancelada con mínimo 2 horas de anticipación");

            var activeMembership = await _clientMembershipRepository.GetActiveMembershipAsync(clientId);

            if (activeMembership != null)
            {
                activeMembership.RemainingClasses++;
            }
                
            _enrollmentRepository.Remove(enrollment);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
