using EvolCep.Shared.Dtos.Sessions;
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
            if (dto == null)
                return BadRequest("Los datos de la sesión son obligatorios");
            
            await _creationService.CreateAsync(dto);

            return StatusCode(201, new
            {
                message = "Sesión de entrenamiento creada exitosamente",
                scheduleFor = dto.StartDateTime,
                description = dto.Description,
            });
        }
    }
}
