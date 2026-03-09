using EvolCep.Data;
using EvolCep.Dtos.Auth;
using EvolCep.Models;
using EvolCep.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using EvolCep.Constants;
using Microsoft.EntityFrameworkCore;

namespace EvolCep.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context,
            IJwtTokenGenerator jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwt = jwt;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new UnauthorizedAccessException("Credenciales invalidas");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Credenciales invalidas");

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(r => 
                    r.Token == dto.RefreshToken &&
                    !r.IsRevoked) ?? throw new UnauthorizedAccessException("Refresh Token inválido");

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expirado");

            refreshToken.IsRevoked = true;  

            var newAccessToken = await GenerateTokensAsync(refreshToken.ApplicationUser!);

            await _context.SaveChangesAsync();

            return newAccessToken;
        }

        public async Task RegisterClientAsync(RegisterDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                };
                
                var result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                    throw new Exception(string.Join(", ",
                        result.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(user, Roles.Client);

                var client = new Client
                {
                    ApplicationUserId = user.Id,
                    Name = dto.Name,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    WeightKg = dto.WeightKg,
                    PhoneNumber = dto.PhoneNumber
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(ApplicationUser user)
        {
            var accessToken = await _jwt.GenerateTokenAsync(user);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                ApplicationUserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
