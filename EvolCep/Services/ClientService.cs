using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using EvolCep.Dtos.Client;

namespace EvolCep.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Client> GetMyProfileAsync (int clientId)
        {
            return await _context.Clients
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(c => c.Id == clientId)
                ?? throw new Exception("Cliente no encontrado");
        }

        public async Task UpdateMyProfileAsync(int clientId, UpdateClientDto dto)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId)
                ?? throw new Exception("Cliente no encontrado");

            client.Name = dto.Name;
            client.LastName = dto.LastName;
            client.PhoneNumber = dto.PhoneNumber;
            client.WeightKg = dto.WeightKg;

            await _context.SaveChangesAsync();
        }
    }
}
