using System.ComponentModel.DataAnnotations;

namespace EvolCep.Dtos.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
