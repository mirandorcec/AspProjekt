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
        }

	}
}
