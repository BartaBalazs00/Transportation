using Microsoft.EntityFrameworkCore;
using Transportation.Entities.Entity;

namespace Transportation.Data
{
    public class TransportationContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public TransportationContext(DbContextOptions<TransportationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasData(new Vehicle[]
            {
                new Vehicle
                {
                    PassengerCapacity = 5,
                    RangeKm = 500,
                    Fuel = FuelType.Gasoline
                },
                new Vehicle
                {
                    PassengerCapacity = 7,
                    RangeKm = 600,
                    Fuel = FuelType.Electric
                },
                new Vehicle
                {
                    PassengerCapacity = 4,
                    RangeKm = 300,
                    Fuel = FuelType.Hybrid
                }
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
