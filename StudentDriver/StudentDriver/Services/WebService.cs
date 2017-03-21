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
using OAuth;
using OAuthAccess;
using StudentDriver.Helpers;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace StudentDriver.Services
{
    public class WebService
    {
        private static HttpClient _client;
        private static WebService _instance;
        private OAuthController _oAuthController;

        public static WebService Instance => _instance ?? (_instance = new WebService());


        private WebService()
        {
            _client = new HttpClient();
            _oAuthController = new OAuthController();
        }

        //public async Task<UserStats> GetStudentStats(int id)
        //{
        //    var requestUri = GenerateRequestUri(_apiBaseUrl, "students", new Dictionary<string, string>() {{"userId", id.ToString()}});
        //    var response = await _client.GetAsync(requestUri);
        //    var json = response.Content.ReadAsStringAsync().Result;
        //    var userStats = JsonConvert.DeserializeObject<UserStats>(json);
        //    return userStats;
        //}

        //public async Task<bool> PostUnsyncDrivingSessions(List<UnsyncDrive> unsyncDrives)
        //{
        //    var json = new JObject(new JProperty("unsyncDrives", unsyncDrives));

        //    var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        //    var requestUri = GenerateRequestUri(_apiBaseUrl, "drivingsessions");
        //    var response = await _client.PostAsync(requestUri, content);
        //    return response.IsSuccessStatusCode;
        //}

        //public async Task<List<StateReqs>> GetStateReqs()
        //{
        //    var requestUri = GenerateRequestUri(_apiBaseUrl, "statereqs");
        //    var response = await _client.GetAsync(requestUri);
        //    var json = response.Content.ReadAsStringAsync().Result;
        //    var stateReqs = JsonConvert.DeserializeObject<List<StateReqs>>(json);
        //    return stateReqs;
        //}

        // TODO still need to define the WeatherData Object
        public async Task<Object> GetWeatherData(double latitude, double longitude)
        {
            var requestUri = GenerateDarkSkyWeatherRequestUri("", latitude, longitude);
            var response = await _client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject(json);
            return weatherData;
        }

        public async Task<bool> SaveAccount (AccountDummy dummyAccount)
        {
            var account = new Account(dummyAccount.Username,dummyAccount.Properties,dummyAccount.Cookies); 
            if (await VerifiedAccountAgainstFacebook(account))
            {
                await SaveAccountToDevice(account);
            }
            return true;
        }

        private async Task<bool> VerifiedAccountAgainstFacebook(Account account)
        {
            var url = GenerateRequestUrl(Settings.APIBaseUrl, Settings.OAuthEndpoint);
            var response = await _oAuthController.MakePostRequest("https://dev.drivinglog.online/auth/facebook/token", account);
            return response?.StatusCode == HttpStatusCode.OK;
        }

        private async Task<bool> SaveAccountToDevice(Account account)
        {
            var response = await _oAuthController.GetProfile(account);
            var json = response.GetResponseText();
            return true;


        }

        public async Task<bool> Logout ()
        {
   //         _oAuthController.LogOut();

			//string url = "";
			//if (Settings.OAuthSourceProvider == OAuthSource.Facebook) {
			//	url = string.Format ("https://facebook.com/logout.php?next={0}&access_token={1}", WebService._apiBaseUrl, Settings.OAuthAccessToken);
			//}
			//var response = await _client.GetAsync (url);
			//if (response.IsSuccessStatusCode) {
			//	Settings.OAuthSourceProvider = OAuthSource.None;
			//	Settings.OAuthAccessToken = "";
			//	this.SetTokenHeader ();
			//	return true;
			//}
			return false;

		}


		private string GenerateRequestUrl(string host, string endPoint, Dictionary<string, string> paramDictionary = null)
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
