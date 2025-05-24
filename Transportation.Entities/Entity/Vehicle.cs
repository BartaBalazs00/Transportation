using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transportation.Entities.Helper;

namespace Transportation.Entities.Entity
{
    public enum FuelType
    {
        Gasoline,
        Electric,
        Hybrid
    }
    public class Vehicle :IIdentity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int PassengerCapacity { get; set; }
        public int RangeKm { get; set; }
        public FuelType Fuel { get; set; } = FuelType.Gasoline;
    }
}
