using EvolCep.Dtos;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvolCep.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/workout-sessions")]
    public class WorkoutSessionAdminController : ControllerBase
    {
        private readonly IWorkoutSessionService _service;

        public WorkoutSessionAdminController(IWorkoutSessionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create (CreateWorkoutSessionDto dto)
        {
            await _service.CreateAsync(dto);

            return Ok(new { message = "Clase creada correctamente" });
        }
    }
}
