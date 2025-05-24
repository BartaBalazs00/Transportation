using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Entities.Dto
{
    public class TripRequestDto
    {
        public int Passengers { get; set; }
        public int DistanceKm { get; set; }
    }
}
