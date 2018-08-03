using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelBot
{
    public class FlightRequest
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string DateOfDepa { get; set; }
        public string DateOfArri { get; set; }
        public string SeatingClass { get; set; } = "E";
        public int? Adult { get; set; }
        public int? Childern { get; set; } = 0;
        public int? infants { get; set; } = 0;
        public int Counter { get; set; } = 100;

    }
}