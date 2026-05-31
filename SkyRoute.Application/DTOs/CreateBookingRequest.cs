using SkyRoute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoute.Application.DTOs
{
    public class CreateBookingRequest
    {
        public Flight Flight { get; set; }
        public decimal Price { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
