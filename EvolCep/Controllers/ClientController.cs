using EvolCep.Shared.Dtos.Clients;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EvolCep.Extensions;

namespace EvolCep.Controllers
{
    [ApiController]
    [Route("api/clients")]
    [Authorize(Roles = "Client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var clientId = User.GetIdClient();

            var client = await _clientService.GetMyProfileAsync(clientId);

            if (client == null)
                return NotFound(new { message = "Perfil de usuario no encontrado." });

            return Ok(client);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody]UpdateClientDto dto)
        {
            var clientId = User.GetIdClient();

            await _clientService.UpdateMyProfileAsync(clientId, dto);

            return NoContent();
        }
    }
}
