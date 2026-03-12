using EvolCep.Dtos;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvolCep.Controllers
{
    [ApiController]
    [Route("api/Membership")]
    [Authorize(Roles = "Client")]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpPost("buy/{membershipId}")]
        public async Task<IActionResult> BuyMembership([FromBody] BuyMembershipDto dto)
        {
            var clientId = GetClientId();

            await _membershipService.BuyAsync(clientId, dto.MembershipId);

            return Ok(new
            {
                Message = "Membresía comprada exitosamente"
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableMemberships()
        {
            var memberships = await _membershipService.GetAvailableMemberShipAsync();
            return Ok(memberships);
        }

        [HttpGet("my-membership")]
        [Authorize]
        public async Task<IActionResult> MyMembership()
        {
            var clientId = GetClientId();

            var membership = await _membershipService.GetMyMembershipAsync(clientId);

            if (membership == null)
                return NotFound("No tienes membresía activa");

            return Ok(membership);
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> MembershipHistory()
        {
            var clientId = GetClientId();

            var history = await _membershipService.GetHistoryAsync(clientId);

            return Ok(history);
        }

        private int GetClientId()
        {
            var claim = User.FindFirst("ClientId")?.Value
                ?? throw new Exception("ClientId no encontrado en el token");

            return int.Parse(claim);
        }
    }
}
