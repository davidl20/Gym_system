using EvolCep.Models;
using EvolCep.Dtos.Client;

namespace EvolCep.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientProfileDto> GetMyProfileAsync(int clientId);
        Task UpdateMyProfileAsync (int clientId, UpdateClientDto dto);
    }
}
