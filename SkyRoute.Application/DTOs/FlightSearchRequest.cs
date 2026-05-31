using SkyRoute.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoute.Application.DTOs
{
    public class FlightSearchRequest
    {
        public List<FlightLegRequest> Legs { get; set; } = [];
        public int Passengers { get; set; }
        public CabinClass CabinClass { get; set; }
        public FlightType FlightType { get; set; }
        public bool FlexDates { get; set; }
    }
}
