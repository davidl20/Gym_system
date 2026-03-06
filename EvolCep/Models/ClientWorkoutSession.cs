namespace EvolCep.Models
{
    public class ClientWorkoutSession
    {
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public int WorkoutSessionId { get; set; }
        public WorkoutSession WorkoutSession { get; set; } = null!;
        public DateTime StartDateTime { get; set; } 
    }
}
