using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelBot.Data
{

    public class AirportModel
    {
        public Airport[] airports { get; set; }
    }

    public class Airport
    {
        public string IATA_code { get; set; }
        public string ICAO_code { get; set; }
        public string airport_name { get; set; }
        public string city_name { get; set; }
    }

}