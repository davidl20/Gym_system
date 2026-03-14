namespace EvolCep.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalClasses { get; set; } 
        public int DurationInDays { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<ClientMembership> ClientsMemberships { get; } = new List<ClientMembership>();
    }
}
