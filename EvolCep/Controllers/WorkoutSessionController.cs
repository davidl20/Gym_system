using EvolCep.Dtos;
using EvolCep.Services.Interfaces;
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
        private readonly IWorkoutSessionService _service;

        public WorkoutSessionController(IWorkoutSessionService service)
        {
            _service = service;
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll (EnrollWorkoutSessionDto dto)
        {
            var clientId = User.GetIdClient();

            await _service.EnrollAsync (clientId, dto.WorkoutSessionId);

            return Ok(new { message = "Agendamiento exitoso" });
        }

        [HttpDelete("cancel")]
        public async Task<IActionResult> Cancel (CancelWorkoutSessionDto dto)
        {
            var clientId = User.GetIdClient();

            await _service.CancelAsync(
                clientId,
                dto.WorkoutSessionId);

            return Ok(new { message = "Clase agendada cancelada correctamente" });
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodaySessions()
        {
            var clientId = User.GetIdClient ();

            var sessions = await _service.GetTodaySessionsAsync(clientId);
            return Ok(sessions);
        }
    }
}
