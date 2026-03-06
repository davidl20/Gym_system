using EvolCep.Models;

namespace EvolCep.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
