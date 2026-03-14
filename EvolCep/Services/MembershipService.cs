using EvolCep.Dtos;
using EvolCep.Models;
using EvolCep.Repositories.Interfaces;
using EvolCep.Services.Interfaces;

namespace EvolCep.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientMembershipRepository _clientMembershipRepository;

        public MembershipService(
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork,
            IClientMembershipRepository clientMembershipRepository)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
            _clientMembershipRepository = clientMembershipRepository;
        }
        public async Task BuyAsync(int clientId, int membershipId)
        {
            var plan = await _membershipRepository.GetByIdAsync(membershipId)
                ?? throw new KeyNotFoundException("El plan de membresía seleccionado no existe");

            var activeMembership = await _clientMembershipRepository.GetActiveMembershipAsync(clientId);

            if (activeMembership != null)
                throw new InvalidOperationException("Ya tienes una membresía activa.");

            DateTime startDate = DateTime.UtcNow;

            int daysToAdd = plan.DurationInDays > 0 ? plan.DurationInDays : 30;
            DateTime endDate = startDate.AddDays(daysToAdd);

            var newMembership = new ClientMembership
            {
                ClientId = clientId,
                MembershipPlanId = plan.Id,
                RemainingClasses = plan.TotalClasses,
                StartDate = startDate,
                EndDate = endDate
            };

            await _clientMembershipRepository.AddAsync(newMembership);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MembershipDto>> GetAvailableMemberShipAsync()
        {
            var memberships = await _membershipRepository.GetAllAsync();

            return memberships.Select(m => new MembershipDto
            {
                Description = m.Description,
                TotalClasses = m.TotalClasses,
                Price = m.Price
            });
        }

        public async Task<IEnumerable<MyMembershipDto>> GetHistoryAsync(int clientId)
        {
            var history = await _clientMembershipRepository.GetHistoryAsync(clientId);

            return history.Select(MapToMyMembershipDto);
        }

        public async Task<MyMembershipDto?> GetMyMembershipAsync(int clientId)
        {
            var membership = await _clientMembershipRepository.GetActiveMembershipAsync(clientId);

            if (membership == null)
                return null;

            return MapToMyMembershipDto(membership);
        }

        private MyMembershipDto MapToMyMembershipDto(ClientMembership m)
        {
            return new MyMembershipDto
            {
                Description = m.MembershipPlan?.Description ?? "Plan sin descripción",
                RemainingClasses = m.RemainingClasses,
                StartDate = m.StartDate,
                EndDate = m.EndDate
            };
        }
    }
}
