namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionEnrollmentService
    {
        Task EnrollAsync(int clientId, int workoutSessionId);
    }
}
