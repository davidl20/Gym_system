using EvolCep.Shared.Dtos.Memberships;

namespace EvolCep.Shared.Dtos.Clients
{
    public class ClientProfileDto
    {
        public string Document { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal? WeightKg { get; set; }
        public MyMembershipDto? ActiveMembership { get; set; }
    }
}
