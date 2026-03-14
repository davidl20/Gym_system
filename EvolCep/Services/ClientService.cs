using EvolCep.Dtos;
using EvolCep.Services.Interfaces;
using EvolCep.Dtos.Client;
using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientMembershipRepository _clientMembershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(
            IClientRepository clientRepository,
            IClientMembershipRepository clientMembershipRepository,
            IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _clientMembershipRepository = clientMembershipRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ClientProfileDto> GetMyProfileAsync (int clientId)
        {
            var client = await _clientRepository.GetByIdAsync (clientId)
                ?? throw new Exception("Cliente no encontrado");

            var activeMembership = await _clientMembershipRepository.GetActiveMembershipAsync(clientId);

            return new ClientProfileDto
            {
                Document = client.Document,
                Name = client.Name,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                WeightKg = client.WeightKg,
                ActiveMembership = activeMembership == null ? null : new MyMembershipDto
                {
                    Description = activeMembership.MembershipPlan?.Description ?? "Plan activo",
                    RemainingClasses = activeMembership.RemainingClasses,
                    EndDate= activeMembership.EndDate,
                } 
            };

        }

        public async Task UpdateMyProfileAsync(int clientId, UpdateClientDto dto)
        {
            var client = await _clientRepository.GetByIdAsync(clientId)
                ?? throw new Exception("Cliente no encontrado");

            client.Name = dto.Name;
            client.LastName = dto.LastName;
            client.PhoneNumber = dto.PhoneNumber;
            client.WeightKg = dto.WeightKg;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
