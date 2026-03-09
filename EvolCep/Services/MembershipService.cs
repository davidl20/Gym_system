using EvolCep.Repositories.Interfaces;
using EvolCep.Services.Interfaces;

namespace EvolCep.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MembershipService(
            IClientRepository clientRepository,
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task BuyAsync(int clientId, int membershipId)
        {
            var client = await _clientRepository.GetClientWithMembershipAsync(clientId) ??
                throw new Exception("El cliente no existe");

            if (client.Membership != null &&
                client.Membership.EndDate >= DateTime.UtcNow &&
                client.Membership.RemainingClasses > 0)
            {
                throw new Exception("El cliente ya tiene una membresía activa");
            }

            var membership = await _membershipRepository.GetByIdAsync(membershipId) ??
                 throw new Exception("La membresía no existe");

            // Activar membresía
            membership.StartDate = DateTime.Now;
            membership.EndDate = DateTime.Now.AddMonths(1);
            membership.RemainingClasses = membership.TotalClasses;

            client.Membership = membership;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
