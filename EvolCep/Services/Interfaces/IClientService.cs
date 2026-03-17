using EvolCep.Shared.Dtos.Clients;

namespace EvolCep.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientProfileDto> GetMyProfileAsync(int clientId);
        Task UpdateMyProfileAsync (int clientId, UpdateClientDto dto);
    }
}
