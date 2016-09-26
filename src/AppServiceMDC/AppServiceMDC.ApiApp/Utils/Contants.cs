using System.Configuration;

namespace AppServiceMDC.ApiApp.Utils
{
    internal class Contants
    {
        public static string AuthString = ConfigurationManager.AppSettings["AuthString"];
        public static string ClientId = ConfigurationManager.AppSettings["ClientId"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        public static string ResAzureGraphApi = ConfigurationManager.AppSettings["ResAzureGraphApi"];
        public static string ServiceRootUrl = ConfigurationManager.AppSettings["ServiceRootUrl"];
    }
}