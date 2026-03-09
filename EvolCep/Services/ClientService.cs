using EvolCep.Models;
using EvolCep.Services.Interfaces;
using EvolCep.Dtos.Client;
using EvolCep.Repositories.Interfaces;

namespace EvolCep.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(
            IClientRepository clientRepository, 
            IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Client> GetMyProfileAsync (int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);

           return client ?? throw new Exception("Cliente no encontrado");
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
