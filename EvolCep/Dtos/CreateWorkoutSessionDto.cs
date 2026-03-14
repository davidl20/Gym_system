namespace EvolCep.Dtos
{
    public class CreateWorkoutSessionDto
    {
        public string Description { get; set; } = string.Empty; 
        public DateTime StartDateTime { get; set; }
        public int? MaxClients { get; set; } = 10;
        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);
    }
}
