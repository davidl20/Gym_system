namespace EvolCep.Dtos
{
    public class MyMembershipDto
    {
        public string Description { get; set; } = string.Empty;
        public int RemainingClasses { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
