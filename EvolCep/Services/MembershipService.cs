using EvolCep.Dtos;
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
            var client = await _clientRepository.GetClientWithMembershipAsync(clientId);

            if(client?.Membership == null)
                return Enumerable.Empty<MyMembershipDto>();

            return new List<MyMembershipDto>
            {
                new MyMembershipDto
                {
                    Description = client.Membership.Description,
                    RemainingClasses = client.Membership.RemainingClasses,
                    StartDate = client.Membership.StartDate,
                    EndDate = client.Membership.EndDate
                }
            };
        }

        public async Task<MyMembershipDto> GetMyMembershipAsync(int clientId)
        {
            var client = await _clientRepository.GetClientWithMembershipAsync(clientId);

            if (client?.Membership == null)
                return null;

            return new MyMembershipDto
            {
                Description = client.Membership.Description,
                RemainingClasses = client.Membership.RemainingClasses,
                StartDate = client.Membership.StartDate,
                EndDate = client.Membership.EndDate
            };
        }

    }
}
