using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Entities.Entity;

namespace Transportation.Entities.Dto
{
    public class TripSuggestionDto
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public double Profit { get; set; }
    }
}
