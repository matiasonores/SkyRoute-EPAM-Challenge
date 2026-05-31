using Microsoft.EntityFrameworkCore;
using SkyRoute.Domain.Entities;
using System.Reflection;

namespace SkyRoute.Infraestructure.Persistence
{
    public class SkyRouteDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Country> Countries { get; set; }

        public SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
