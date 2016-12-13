using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using StudentDriver.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDriver.Helpers;

namespace StudentDriver.Services
{
    public static class WebService
    {
        private static string ServiceApiBaseUri = Config.ServiceApiBaseUri;

        public static async Task<string> GetAuthentiationToken(OAuthProvider.ProviderType providerType, string OAuthToken)
        {
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(ServiceApiBaseUri, Config.OAuthProviderAuthenicateEndPoint_GET, new Dictionary<string, string>() { { Config.OAuthProviderTypeKey, providerType.ToString() }, { Config.OAuthProviderAuthTokenKey, OAuthToken } });
            var response = await client.GetAsync(requestUri);
            if (response.StatusCode != HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return null;
            }
            var json = response.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<string>(json);
            return token;
        }

        public static async Task<UserStats> GetStudentStats(int id)
        {
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(ServiceApiBaseUri,Config.StudentStatsEndPoint_GET, new Dictionary<string, string>() {{Config.StudentStatsUserIdKey, id.ToString()}});
            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var userStats = JsonConvert.DeserializeObject<UserStats>(json);
            return userStats;
        }

        public static async Task<bool> PostUnsyncDrivingSessions(List<UnsyncDrive> unsyncDrives)
        {
            var json = new JObject(new JProperty(Config.UnsynDrivingSessionsUnsyncDrivesKey,unsyncDrives));

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(ServiceApiBaseUri,Config.UnsycDrivingSessionsEndPoint_POST);

            var response = await client.PostAsync(requestUri, content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<List<StateReqs>> GetStateReqs()
        {
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(ServiceApiBaseUri, Config.StateRegsEndpoint_GET);

            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var stateReqs = JsonConvert.DeserializeObject<List<StateReqs>>(json);
            return stateReqs;
        }

        public static async Task<DriveWeatherData> GetDriveWeatherData(double latitude, double longitude)
        {
            var client = new HttpClient();
            var requestUri = GenerateDarkSkyWeatherRequestUri(Config.DarkSkyWeatherApiKey,latitude,longitude);
            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject<DriveWeatherData>(json);
            return weatherData;
        }



        private static string GenerateRequestUri(string host, string endPoint, Dictionary<string, string> paramDictionary = null)
        {
            var queryString = string.Format(string.Join("&", paramDictionary.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            var builder = new UriBuilder
                          {
                              Host = host,
                              Path = endPoint,
                              Query = queryString
                          };
            return builder.ToString();

        }

        private static string GenerateDarkSkyWeatherRequestUri(string apiKey, double latitude, double longitude)
        {
            return string.Join(",",string.Join("/", Config.DarkSkyWeatherApiEndpoint, Config.DarkSkyWeatherApiForecastKey, apiKey),latitude,longitude);
        }
    }
}
