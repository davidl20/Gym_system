namespace EvolCep.Dtos
{
    public class MembershipDto
    {
        public int Id { get; set; }
        //public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TotalClasses { get; set; }
        public decimal Price { get; set; }
    }
}
