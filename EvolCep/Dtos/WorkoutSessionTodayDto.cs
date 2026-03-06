namespace EvolCep.Dtos
{
    public class WorkoutSessionTodayDto
    {
        public int WorkoutSessionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int AvailableSpots { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
