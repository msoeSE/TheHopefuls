using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using StudentDriver.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDriver.Helpers;
using Xamarin.Auth;

namespace StudentDriver.Services
{
    public class WebService
    {

        public enum OAuthSource
        {
            None,
            Facebook,
        }

        private static string ApiBaseUrl = "";
        private static int ApiBasePort = 3000;

        private static HttpClient _client;
        private static WebService _service;

        public static WebService GetInstance()
        {
#if DEBUG
            ApiBaseUrl = "dev.drivinglog.online/";

#else

		ApiBaseUrl = "drivinglog.online/";
#endif

            return _service ?? (_service = new WebService());

        }

        private WebService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("access_token", "");
        }

        public void SetTokenHeader()
        {
            _client.DefaultRequestHeaders.Remove("access_token");
            _client.DefaultRequestHeaders.Add("access_token", Settings.OAuthAccessToken);
        }

        public async Task<UserStats> GetStudentStats(int id)
        {
            var requestUri = GenerateRequestUri(ApiBaseUrl, "students", new Dictionary<string, string>() {{"userId", id.ToString()}});
            var response = await _client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var userStats = JsonConvert.DeserializeObject<UserStats>(json);
            return userStats;
        }

        public async Task<bool> PostUnsyncDrivingSessions(List<UnsyncDrive> unsyncDrives)
        {
            var json = new JObject(new JProperty("unsyncDrives", unsyncDrives));

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var requestUri = GenerateRequestUri(ApiBaseUrl, "drivingsessions");
            var response = await _client.PostAsync(requestUri, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<StateReqs>> GetStateReqs()
        {
            var requestUri = GenerateRequestUri(ApiBaseUrl, "statereqs");
            var response = await _client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var stateReqs = JsonConvert.DeserializeObject<List<StateReqs>>(json);
            return stateReqs;
        }

        // TODO still need to define the WeatherData Object
        public async Task<Object> GetWeatherData(double latitude, double longitude)
        {
            var requestUri = GenerateDarkSkyWeatherRequestUri("", latitude, longitude);
            var response = await _client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject(json);
            return weatherData;
        }

        public async Task<bool> PostOAuthToken (OAuthSource source, string token)
		{
			if (string.IsNullOrEmpty (token)) {
				return false;
			}
			string endpoint = "";
			if (source == OAuthSource.Facebook) {
				endpoint = "auth/facebook/token";
			}
			Settings.OAuthSourceProvider = source;
			var json = new JObject (new JProperty ("access_token", token));
			var content = new StringContent (json.ToString (), Encoding.UTF8, "application/json");
			var uri = GenerateRequestUri (ApiBaseUrl, endpoint);
			var response = await _client.PostAsync (uri, content).ConfigureAwait (false);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> OAuthLogout ()
		{
			string url = "";
			if (Settings.OAuthSourceProvider == OAuthSource.Facebook) {
				url = string.Format ("https://facebook.com/logout.php?next={0}&access_token={1}", WebService.ApiBaseUrl, Settings.OAuthAccessToken);
			}
			var response = await _client.GetAsync (url);
			if (response.IsSuccessStatusCode) {
				Settings.OAuthSourceProvider = OAuthSource.None;
				Settings.OAuthAccessToken = "";
				this.SetTokenHeader ();
				return true;
			}
			return false;

		}


		private string GenerateRequestUri (string host, string endPoint, Dictionary<string, string> paramDictionary = null)
		{
			var queryString = string.Empty;
			if (paramDictionary != null) {
				queryString = string.Format (string.Join ("&", paramDictionary.Select (kvp => $"{kvp.Key}={kvp.Value}")));
			}

			var builder = new UriBuilder {
				Host = host,
				Path = endPoint,
				Query = queryString
			};
			return builder.ToString ();

		}

		private string GenerateDarkSkyWeatherRequestUri (string apiKey, double latitude, double longitude)
		{
			return string.Join (",", string.Join ("/", "https://api.darksky.net", "forecast", apiKey), latitude, longitude);
		}
	}
}
