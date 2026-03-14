using System.ComponentModel.DataAnnotations;

namespace EvolCep.Models
{
    public class WorkoutSession
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration {  get; set; } = TimeSpan.FromHours(1);
        public int MaxClients { get; set; } = 10;
        public DateTime StartDateTime { get; set; }
        public ICollection<ClientWorkoutSession> ClientWorkoutSessions { get; } = new List<ClientWorkoutSession>();
    }
}
