using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Entities.Entity;

namespace Transportation.Entities.Dto
{
    public class VehicleCreateUpdateDto
    {
        [Required]
        public int PassengerCapacity { get; set; }
        [Required]
        public int RangeKm { get; set; }
        [Required]
        public FuelType Fuel { get; set; } = FuelType.Gasoline;
    }
}
