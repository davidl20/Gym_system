using EvolCep.Services.WorkSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EvolCep.Extensions;

namespace EvolCep.Controllers
{
    [ApiController]
    [Authorize(Roles = "Client")]
    [Route("api/workout-sessions")]
    public class WorkoutSessionController : ControllerBase
    {
        private readonly IWorkoutSessionQueryService _queryService;
        private readonly IWorkoutSessionEnrollmentService _enrollmentService;
        private readonly IWorkoutSessionCancellationService _cancellationService;

        public WorkoutSessionController(
            IWorkoutSessionQueryService queryService,
            IWorkoutSessionEnrollmentService enrollmentService,
            IWorkoutSessionCancellationService cancellationService)
        {
            _queryService = queryService;
            _enrollmentService = enrollmentService;
            _cancellationService = cancellationService;
        }

        [HttpPost("{sessionId}/enroll")]
        public async Task<IActionResult> Enroll(int sessionId)
        {
            var clientId = User.GetIdClient();

            await _enrollmentService.EnrollAsync(
                clientId,
                sessionId);

            return Ok(new { message = "Agendamiento exitoso" });
        }

        [HttpDelete("{sessionId}/cancel")]
        public async Task<IActionResult> Cancel(int sessionId)
        {
            var clientId = User.GetIdClient();

            await _cancellationService.CancelSessionAsync(sessionId, clientId);

            return Ok(new { message = "La reserva ha sido cancelada" });
        }

        [HttpGet]
        public async Task<IActionResult> GetSessions([FromBody] DateTime? date)
        {
            var clientId = User.GetIdClient ();

            var selectedDate = date ?? DateTime.Now;

            var sessions = await _queryService.GetSessionsByDateAsync(clientId, selectedDate);

            return Ok(sessions);
        }
    }
}
