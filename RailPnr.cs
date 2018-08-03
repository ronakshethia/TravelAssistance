using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelBot
{
    public class ToStation
    {
        public string code { get; set; }
        public double lng { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
    }

    public class Class
    {
        public string code { get; set; }
        public string available { get; set; }
        public string name { get; set; }
    }

    public class Day
    {
        public string code { get; set; }
        public string runs { get; set; }
    }

    public class Train
    {
        public string number { get; set; }
        public List<Class> classes { get; set; }
        public List<Day> days { get; set; }
        public string name { get; set; }
    }

    public class FromStation
    {
        public string code { get; set; }
        public double lng { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
    }

    public class Passenger
    {
        public string booking_status { get; set; }
        public string current_status { get; set; }
        public int no { get; set; }
    }

    public class ReservationUpto
    {
        public string code { get; set; }
        public double lng { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
    }

    public class JourneyClass
    {
        public string code { get; set; }
        public object name { get; set; }
    }

    public class BoardingPoint
    {
        public string code { get; set; }
        public double lng { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
    }

    public class RailPNR
    {
        public int total_passengers { get; set; }
        public string pnr { get; set; }
        public ToStation to_station { get; set; }
        public int debit { get; set; }
        public Train train { get; set; }
        public bool chart_prepared { get; set; }
        public FromStation from_station { get; set; }
        public List<Passenger> passengers { get; set; }
        public ReservationUpto reservation_upto { get; set; }
        public string doj { get; set; }
        public JourneyClass journey_class { get; set; }
        public BoardingPoint boarding_point { get; set; }
        public int response_code { get; set; }
    }
}