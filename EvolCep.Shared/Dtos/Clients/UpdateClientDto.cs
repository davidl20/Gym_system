namespace EvolCep.Shared.Dtos.Clients
{
    public class UpdateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int WeightKg { get; set; }
    }
}
