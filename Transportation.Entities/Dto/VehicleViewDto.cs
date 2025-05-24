using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Entities.Entity;

namespace Transportation.Entities.Dto
{
    public class VehicleViewDto
    {
        public string Id { get; set; } = "";
        public int PassengerCapacity { get; set; }
        public int RangeKm { get; set; }
        public FuelType Fuel { get; set; } = FuelType.Gasoline;
    }
}
