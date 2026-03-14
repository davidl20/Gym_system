using System.ComponentModel.DataAnnotations;

namespace EvolCep.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Document { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty ;
        public DateTime BirthDate { get; set; }
        public decimal WeightKg { get; set; }
        public string Email { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        //Membership
        public ICollection<ClientMembership> Memberships { get; } = [];

        //Idntity relation
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public ICollection<ClientWorkoutSession> ClientWorkoutSessions { get; } = [];

    }
}
