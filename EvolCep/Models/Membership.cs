namespace EvolCep.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public int TotalClasses { get; set; } //Clases del plan
        public int RemainingClasses { get; set; } //Clases disponibles
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } //Fecha de vencimiento
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<Client> Clients { get; } = new List<Client>();
    }
}
