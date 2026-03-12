namespace EvolCep.Models
{
    public class ClientMembership
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public int MembershipPlanId { get; set; }
        public Membership MembershipPlan { get; set; } = null!;
        public int RemainingClasses { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
