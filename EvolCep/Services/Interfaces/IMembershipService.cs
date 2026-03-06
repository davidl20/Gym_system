namespace EvolCep.Services.Interfaces
{
    public interface IMembershipService
    {
        Task BuyAsync(int clientId, int membershipId);
    }
}
