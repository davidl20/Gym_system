using System.ComponentModel.DataAnnotations;

namespace EvolCep.Dtos.Auth
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public decimal WeightKg { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
