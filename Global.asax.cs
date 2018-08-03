using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TravelBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        internal static HttpClient httpClientInstance;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);


            Uri baseUri = new Uri("http://developer.goibibo.com/api/");
            httpClientInstance = new HttpClient();
            httpClientInstance.BaseAddress = baseUri;
            httpClientInstance.DefaultRequestHeaders.Clear();



            httpClientInstance.DefaultRequestHeaders.ConnectionClose = false;
            httpClientInstance.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.FindServicePoint(baseUri).ConnectionLeaseTimeout = 60 * 1000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        }
    }
}
