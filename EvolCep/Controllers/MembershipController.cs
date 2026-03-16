using EvolCep.Dtos;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EvolCep.Extensions;

namespace EvolCep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyMembership([FromBody] BuyMembershipDto dto)
        {
            var clientId = User.GetIdClient();

            await _membershipService.BuyAsync(clientId, dto.MembershipId);

            return Ok(new
            {
                Message = "Membresía comprada exitosamente. Ya puedes agendar tus clases."
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
            var clientId = User.GetIdClient();

            var membership = await _membershipService.GetMyMembershipAsync(clientId);

            if (membership == null)
                return NotFound("No tienes membresía activa");

            return Ok(membership);
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> MembershipHistory()
        {
            var clientId = User.GetIdClient();

            var history = await _membershipService.GetHistoryAsync(clientId);

            return Ok(history);
        }
    }
}
