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
			Facebook,
			Google
		}
		private static string BaseUrl = "";

		private static HttpClient client;
		private static WebService service;

		public static WebService GetInstance ()
		{
			return service ?? (service = new WebService ());
		}

		private WebService ()
		{
			client = new HttpClient ();
			client.DefaultRequestHeaders.Add ("access_token", "");
		}

		public void SetTokenHeader (string token)
		{
			if (!string.IsNullOrEmpty (token)) {
				client.DefaultRequestHeaders.Remove ("access_token");
				client.DefaultRequestHeaders.Add ("access_token", token);
			}
		}

		public async Task<UserStats> GetStudentStats (int id)
		{
			var requestUri = GenerateRequestUri (BaseUrl, "students", new Dictionary<string, string> () { { "userId", id.ToString () } });
			var response = await client.GetAsync (requestUri);
			var json = response.Content.ReadAsStringAsync ().Result;
			var userStats = JsonConvert.DeserializeObject<UserStats> (json);
			return userStats;
		}

		public async Task<bool> PostUnsyncDrivingSessions (List<UnsyncDrive> unsyncDrives)
		{
			var json = new JObject (new JProperty ("unsyncDrives", unsyncDrives));

			var content = new StringContent (json.ToString (), Encoding.UTF8, "application/json");
			var requestUri = GenerateRequestUri (BaseUrl, "drivingsessions");
			var response = await client.PostAsync (requestUri, content);
			return response.IsSuccessStatusCode;
		}

		public async Task<List<StateReqs>> GetStateReqs ()
		{
			var requestUri = GenerateRequestUri (BaseUrl, "statereqs");
			var response = await client.GetAsync (requestUri);
			var json = response.Content.ReadAsStringAsync ().Result;
			var stateReqs = JsonConvert.DeserializeObject<List<StateReqs>> (json);
			return stateReqs;
		}

		// TODO still need to define the WeatherData Object
		public async Task<Object> GetWeatherData (double latitude, double longitude)
		{
			var requestUri = GenerateDarkSkyWeatherRequestUri ("", latitude, longitude);
			var response = await client.GetAsync (requestUri);
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
			var json = new JObject (new JProperty ("accept_token", token));
			var content = new StringContent (json.ToString (), Encoding.UTF8, "application/json");
			var response = await client.PostAsync (GenerateRequestUri (BaseUrl, endpoint), content);
			if (response.IsSuccessStatusCode) {
				return true;
			}
			return false;
		}


		private string GenerateRequestUri (string host, string endPoint, Dictionary<string, string> paramDictionary = null)
		{
			var queryString = string.Format (string.Join ("&", paramDictionary.Select (kvp => $"{kvp.Key}={kvp.Value}")));

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
