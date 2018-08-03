using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelBot.Helper
{
    public class Settings
    {
        public static readonly string BaseApiUrl = "";
        public static readonly string FlightUrl = "search/?app_id=cc9560d4&app_key=49d2f4b93dfab8f45152329c924df99b&format=json&";
        public static readonly string PnrUrl = "https://api.railwayapi.com/v2/pnr-status/pnr/8101828044/apikey/48t58kwig1/"; 
    }
}