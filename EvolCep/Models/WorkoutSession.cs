using System.ComponentModel.DataAnnotations;

namespace EvolCep.Models
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public TimeSpan Duration {  get; set; } = TimeSpan.FromHours(1);
        public int MaxClients { get; set; } = 10;
        public DateTime StartDateTime { get; set; }
        //public DateTime EndDateTime { get; set; }
        public ICollection<ClientWorkoutSession> ClientWorkoutSessions { get; } = new List<ClientWorkoutSession>();
    }
}
