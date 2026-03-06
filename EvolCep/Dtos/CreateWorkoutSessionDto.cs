namespace EvolCep.Dtos
{
    public class CreateWorkoutSessionDto
    {
        public DateTime StartDateTime { get; set; }
        public int? MaxClients { get; set; }
        public TimeSpan? Duration { get; set; } 
    }
}
