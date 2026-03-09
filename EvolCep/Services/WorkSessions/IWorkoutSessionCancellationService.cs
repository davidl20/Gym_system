namespace EvolCep.Services.WorkSessions
{
    public interface IWorkoutSessionCancellationService
    {
        Task CancelSessionAsync(int sessionId, int clientId);
    }
}
