using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelBot
{
    public class Fare
    {
        public int grossamount { get; set; }
        public int totalbasefare { get; set; }
        public int adultbasefare { get; set; }
        public int totalfare { get; set; }
        public int totalsurcharge { get; set; }
        public int totaltaxes { get; set; }
        public int adulttax { get; set; }
        public int adulttotalfare { get; set; }
        public string totalcommission { get; set; }
    }

    public class Onwardflight
    {
        public string origin { get; set; }
        public int rating { get; set; }
        public string DepartureTime { get; set; }
        public string flightcode { get; set; }
        public string Group { get; set; }
        public string farebasis { get; set; }
        public string depterminal { get; set; }
        public string holdflag { get; set; }
        public string CINFO { get; set; }
        public string deptime { get; set; }
        public string codeshare { get; set; }
        public string ibibopartner { get; set; }
        public string duration { get; set; }
        public string platingcarrier { get; set; }
        public string qtype { get; set; }
        public string arrterminal { get; set; }
        public string flightno { get; set; }
        public string destination { get; set; }
        public string FlHash { get; set; }
        public string stops { get; set; }
        public string seatsavailable { get; set; }
        public string carrierid { get; set; }
        public string airline { get; set; }
        public double FilteringValue { get; set; }
        public string provider { get; set; }
        public string PromotionId { get; set; }
        public Fare fare { get; set; }
        public string CabinClass { get; set; }
        public string warnings { get; set; }
        public double BookabilityValue { get; set; }
        public string ArrivalTime { get; set; }
        public List<object> onwardflights { get; set; }
        public string aircraftType { get; set; }
        public string seatingclass { get; set; }
        public string operatingcarrier { get; set; }
        public string src { get; set; }
        public string CacheKey { get; set; }
        public string splitduration { get; set; }
        public string searchKey { get; set; }
        public string bookingclass { get; set; }
        public string DataSource { get; set; }
        public string multicitysearch { get; set; }
        public string depdate { get; set; }
        public string arrtime { get; set; }
        public string arrdate { get; set; }
        public string TravelTime { get; set; }
        public string aircraftTypecode { get; set; }
    }

    public class Dataa
    {
        public List<object> returnflights { get; set; }
        public List<Onwardflight> onwardflights { get; set; }
    }

    public class Flight
    {
        public Dataa data { get; set; }
        public int data_length { get; set; }
    }
}