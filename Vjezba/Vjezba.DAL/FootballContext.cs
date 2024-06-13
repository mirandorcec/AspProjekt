using Microsoft.EntityFrameworkCore;
using Vjezba.Model;
namespace Vjezba.DAL
{
    public class FootballContext : DbContext
	{
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }


        protected FootballContext() { }

        public FootballContext(DbContextOptions<FootballContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
               .Property(p => p.Position)
               .HasConversion(
                   v => v.ToString(),
                   v => (Position)Enum.Parse(typeof(Position), v));

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Manager)
                .WithOne(m => m.Team)
                .HasForeignKey<Team>(t => t.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);  

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Team)
                .WithOne(t => t.Manager)
                .HasForeignKey<Manager>(m => m.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }

	}
}
