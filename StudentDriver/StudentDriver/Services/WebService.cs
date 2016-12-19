using System.Collections.Generic;
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
        public static async Task<string> GetAuthentiationToken(OAuthProvider.ProviderType providerType, string OAuthToken)
        {
            var client = new HttpClient();
            var requestUri = ""; // get from config
            var response = await client.GetAsync(requestUri);
            if (response.StatusCode != HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return null;
            }
            var json = response.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<string>(json);
            return token;
        }

        public static async Task<UserStats> PostUnsyncDrivingSessions(List<DriveSession> driveSessions)
        {
            var json = new JObject(new JProperty("driveSessions", driveSessions));

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var requestUri = "";  // get from config

            var response = await client.PostAsync(requestUri, content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var userStats = JsonConvert.DeserializeObject<UserStats>(responseContent);
            return userStats;

        }

        public static async Task<List<StateReqs>> GetStateReqs()
        {
            var client = new HttpClient();
            var requestUri = ""; // get from config
            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var stateReqs = JsonConvert.DeserializeObject<List<StateReqs>>(json);
            return stateReqs;
        }

        public static async Task<DriveWeatherData> GetDriveWeatherData(double latitude, double longitude)
        {
            var client = new HttpClient();
            var requestUri = ""; // get from config
            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject<DriveWeatherData>(json);
            return weatherData;
        }
    }
}
