using EvolCep.Data;
using EvolCep.Models;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EvolCep.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public JwtTokenGenerator(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key no configurada");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer no configurado");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience no configurado");

            var expirationHours = int.TryParse(
                _configuration["Jwt:AccessTokenExpirationHours"],
                out var hours) ? hours : 3;
            
            var clientId = await _context.Clients
                .Where (c => c.ApplicationUserId == user.Id)
                .Select (c => c.Id)
                .FirstOrDefaultAsync();

            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Sub,user.Id),
                new Claim (ClaimTypes.Email, user.Email!),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            if(clientId != 0)
            {
                claims.Add (new Claim ("ClientId", clientId.ToString()));
            };

            //Roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add (new Claim (ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var creds = new SigningCredentials
                (key,
                SecurityAlgorithms.HmacSha256
                );

            /*ClienteId (si existe)
            var clientId = await _context.Clients
                .Where(c => c.ApplicationUserId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clientId != 0)
            {
                claims.Add (new Claim ("client_id", clientId.ToString()));
            }*/

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expirationHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
