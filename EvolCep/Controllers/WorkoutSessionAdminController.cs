using EvolCep.Dtos;
using EvolCep.Services.WorkSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvolCep.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/workout-sessions")]
    public class WorkoutSessionAdminController : ControllerBase
    {
        private readonly IWorkoutSessionCreationService _creationService;

        public WorkoutSessionAdminController(IWorkoutSessionCreationService creationService)
        {
            _creationService = creationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create ([FromBody]CreateWorkoutSessionDto dto)
        {
            await _creationService.CreateAsync(dto);

            return Ok(new { message = "Clase creada correctamente" });
        }
    }
}
