using EvolCep.Dtos;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using EvolCep.Services.Interfaces;

namespace EvolCep.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientMembershipRespository _clientMembershipRespository;

        public MembershipService(
            IClientRepository clientRepository,
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork,
            IClientMembershipRespository clientMembershipRespository)
        {
            _clientRepository = clientRepository;
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
            _clientMembershipRespository = clientMembershipRespository;
        }
        public async Task BuyAsync(int clientId, int membershipId)
        {
            var plan = await _membershipRepository.GetByIdAsync(membershipId)
                ?? throw new Exception("Membresía no encontrada");

            var activeMembership = await _clientMembershipRespository.GetActiveMembershipAsync(clientId);

            if (activeMembership != null)
                throw new Exception("Ya tienes una membresía activa. No puedes comprar otra hasta que expire.");

            var newMembership = new ClientMembership
            {
                ClientId = clientId,
                MembershipPlanId = plan.Id,
                RemainingClasses = plan.TotalClasses,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1) // Asumiendo que la membresía dura 1 mes
            };

            await _clientMembershipRespository.AddAsync(newMembership);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MembershipDto>> GetAvailableMemberShipAsync()
        {
            var memberships = await _membershipRepository.GetAllAsync();

            return memberships.Select(m => new MembershipDto
            {
                Id = m.Id,
                Description = m.Description,
                TotalClasses = m.TotalClasses,
                Price = m.Price
            });
        }

        public async Task<IEnumerable<MyMembershipDto>> GetHistoryAsync(int clientId)
        {
            var memberships = await _clientMembershipRespository.GetHistoryAsync(clientId);

            return memberships.Select(m => new MyMembershipDto
            {
                Description = m.MembershipPlan.Description,
                RemainingClasses = m.RemainingClasses,
                StartDate = m.StartDate,
                EndDate = m.EndDate
            });
        }

        public async Task<MyMembershipDto> GetMyMembershipAsync(int clientId)
        {
            var membership = await _clientMembershipRespository.GetActiveMembershipAsync(clientId);

            if (membership == null)
                return null;

            return new MyMembershipDto
            {
                Description = membership.MembershipPlan.Description,
                RemainingClasses = membership.RemainingClasses,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate
            };
        }

    }
}
