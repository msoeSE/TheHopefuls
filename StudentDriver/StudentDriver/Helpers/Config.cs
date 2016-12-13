using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDriver.Helpers
{
    public static class Config
    {
#if DEBUG
        public const string ServiceApiBaseUri = "http://localhost:3000/";
#else
        public const string ServiceApiBaseUri = "http://dylanisawesome.walseth.me/";
#endif
        public const string OAuthProviderAuthenicateEndPoint_GET = "authenticate";
        public const string OAuthProviderTypeKey = "providerType";
        public const string OAuthProviderAuthTokenKey = "oAuthToken";
        public const string StudentStatsEndPoint_GET = "students";
        public const string StudentStatsUserIdKey = "userId";
        public const string UnsycDrivingSessionsEndPoint_POST = "drivingsessions";
        public const string UnsynDrivingSessionsUnsyncDrivesKey = "unsyncDrives";
        public const string StateRegsEndpoint_GET = "statereqs";
        public const string DarkSkyWeatherApiEndpoint = "https://api.darksky.net";
        public const string DarkSkyWeatherApiKey = "";
        public const string DarkSkyWeatherApiForecastKey = "forecast";



    }
}
