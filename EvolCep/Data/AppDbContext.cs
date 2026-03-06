using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EvolCep.Models;

namespace EvolCep.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<ClientWorkoutSession> ClientWorkoutSessions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Usado para validar cupo máximo por sesión y día
            modelBuilder.Entity<ClientWorkoutSession>()
                .HasIndex(x => new { x.WorkoutSessionId, x.StartDateTime });

            modelBuilder.Entity<Client>()
                .HasOne(c => c.ApplicationUser)
                .WithOne()
                .HasForeignKey<Client>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientWorkoutSession>()
                .HasKey(cws => new { cws.ClientId, cws.StartDateTime });

            modelBuilder.Entity<ClientWorkoutSession>()
                .HasOne(cws => cws.Client)
                .WithMany(cws => cws.ClientWorkoutSessions)
                .HasForeignKey(cws => cws.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientWorkoutSession>()
                .HasOne(cws => cws.WorkoutSession)
                .WithMany(ws => ws.ClientWorkoutSessions)
                .HasForeignKey(cws => cws.WorkoutSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Membership>()
                .HasMany(m => m.Clients)
                .WithOne(c => c.Membership)
                .HasForeignKey(m => m.MembershipId);
        }
    }
}
