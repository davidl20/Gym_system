namespace EvolCep.Shared.Dtos.Sessions
{
    public class WorkoutSessionTodayDto
    {
        public int WorkoutSessionId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        //public int AvailableSpots { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
