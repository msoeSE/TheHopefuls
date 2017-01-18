using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StudentDriver.Models;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDriver.Helpers;

namespace StudentDriver.Services
{
	public class WebService
	{

		public enum OAuthSource
		{
            None,
			Facebook,
			Google
		}
		private static string ApiBaseUrl = "192.168.1.67";
	    private static int ApiBasePort = 3000;

		private static HttpClient _client;
		private static WebService _service;

		public static WebService GetInstance ()
		{
			return _service ?? (_service = new WebService ());
		}

		private WebService ()
		{
			_client = new HttpClient ();
			_client.DefaultRequestHeaders.Add ("access_token", "");
        }

		public void SetTokenHeader ()
		{
			_client.DefaultRequestHeaders.Remove ("access_token");
            _client.DefaultRequestHeaders.Add ("access_token", Settings.OAuthAccessToken);
		}

		public async Task<UserStats> GetStudentStats (int id)
		{
			var requestUri = GenerateRequestUri (ApiBaseUrl, "students", new Dictionary<string, string> () { { "userId", id.ToString () } });
			var response = await _client.GetAsync (requestUri);
			var json = response.Content.ReadAsStringAsync ().Result;
			var userStats = JsonConvert.DeserializeObject<UserStats> (json);
			return userStats;
		}

		public async Task<bool> PostUnsyncDrivingSessions (List<UnsyncDrive> unsyncDrives)
		{
			var json = new JObject (new JProperty ("unsyncDrives", unsyncDrives));

			var content = new StringContent (json.ToString (), Encoding.UTF8, "application/json");
			var requestUri = GenerateRequestUri (ApiBaseUrl, "drivingsessions");
			var response = await _client.PostAsync (requestUri, content);
			return response.IsSuccessStatusCode;
		}

		public async Task<List<StateReqs>> GetStateReqs ()
		{
			var requestUri = GenerateRequestUri (ApiBaseUrl, "statereqs");
			var response = await _client.GetAsync (requestUri);
			var json = response.Content.ReadAsStringAsync ().Result;
			var stateReqs = JsonConvert.DeserializeObject<List<StateReqs>> (json);
			return stateReqs;
		}

		// TODO still need to define the WeatherData Object
		public async Task<Object> GetWeatherData (double latitude, double longitude)
		{
			var requestUri = GenerateDarkSkyWeatherRequestUri ("", latitude, longitude);
			var response = await _client.GetAsync (requestUri);
			var json = response.Content.ReadAsStringAsync ().Result;
			var weatherData = JsonConvert.DeserializeObject (json);
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
			} else {
				endpoint = "auth/google/token";
			}
			var json = new JObject (new JProperty ("access_token", token));
			var content = new StringContent (json.ToString (), Encoding.UTF8, "application/json");
			var uri = GenerateRequestUri (ApiBaseUrl, endpoint);
			var response = await _client.PostAsync (uri, content).ConfigureAwait(false);
			return response.IsSuccessStatusCode;
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
				Query = queryString,
                Port = ApiBasePort
            };
			return builder.ToString ();

		}

		private string GenerateDarkSkyWeatherRequestUri (string apiKey, double latitude, double longitude)
		{
			return string.Join (",", string.Join ("/", "https://api.darksky.net", "forecast", apiKey), latitude, longitude);
		}
	}
}
