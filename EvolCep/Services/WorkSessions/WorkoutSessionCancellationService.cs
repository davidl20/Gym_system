using EvolCep.Data;
using EvolCep.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Services.WorkSessions
{
    public class WorkoutSessionCancellationService : IWorkoutSessionCancellationService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;   

        public WorkoutSessionCancellationService( 
            IClientRepository clientRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
        }
        public async Task CancelSessionAsync(int sessionId, int clientId)
        {
            var enrollment = await _clientRepository.GetEnrollmentAsync(clientId, sessionId) ??
                throw new Exception("No tiene clases agendadas");

            var cancellationDeadLine = enrollment.StartDateTime.AddHours(-2);

            if (DateTime.UtcNow > cancellationDeadLine)
                throw new Exception("La clase solo puede ser cancelada con mínimo 2 horas de anticipación");

            var client = await _clientRepository.GetClientWithMembershipAsync(clientId);

            if (client?.Membership != null)
                client.Membership.RemainingClasses ++;

            await _clientRepository.RemoveEnrollmentAsync(enrollment);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
