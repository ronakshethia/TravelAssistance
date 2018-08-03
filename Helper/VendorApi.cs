using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TravelBot.Helper
{
    public static class VendorApi
    {

        public static async Task<string> SearchFlight(FlightRequest request)
        {
            string data = string.Empty;
            try
            {               
                var str = $"source={request.Source}&destination={request.Destination}&dateofdeparture={request.DateOfDepa}&seatingclass={request.SeatingClass}&adults={request.Adult}&children={request.Childern}&infants={request.infants}&counter={request.Counter}";
                var httpContent = new StringContent(str, Encoding.UTF8, "text/xml");
                var response = await WebApiApplication.httpClientInstance.GetAsync(Settings.FlightUrl+str);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public static async Task<string> GetPNR(string pnr)
        {
            string data = string.Empty;
            try
            {
                var url = $"https://api.railwayapi.com/v2/pnr-status/pnr/{pnr}/apikey/48t58kwig1/";
                //var httpContent = new StringContent(str, Encoding.UTF8, "text/xml");
                var response = await WebApiApplication.httpClientInstance.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

    }
}