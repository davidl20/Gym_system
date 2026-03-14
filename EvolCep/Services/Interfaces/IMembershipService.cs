using EvolCep.Dtos;

namespace EvolCep.Services.Interfaces
{
    public interface IMembershipService
    {
        Task BuyAsync(int clientId, int membershipId);
        Task<IEnumerable<MembershipDto>> GetAvailableMemberShipAsync();
        Task<MyMembershipDto?> GetMyMembershipAsync(int clientId);
        Task<IEnumerable<MyMembershipDto>> GetHistoryAsync(int clientId);    
    }
}
