namespace EvolCep.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalClasses { get; set; } //Clases del plan
        //public int RemainingClasses { get; set; } //Clases disponibles
        //public DateTime StartDate { get; set; }cd
        //public DateTime EndDate { get; set; } //Fecha de vencimiento
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<ClientMembership> ClientsMemberships { get; } = new List<ClientMembership>();
    }
}
