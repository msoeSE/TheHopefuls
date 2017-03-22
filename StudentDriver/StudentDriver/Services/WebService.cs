﻿using System;
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
        private readonly OAuthController _oAuthController;
        private readonly DatabaseController _databaseController;


        public static WebService Instance => _instance ?? (_instance = new WebService());


        private WebService()
        {
            _client = new HttpClient();
            _oAuthController = new OAuthController();
            _databaseController = new DatabaseController();
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

        public async Task<bool> UserLoggedIn()
        {
            var responseText = await _oAuthController.VerifySavedAccount(Settings.OAuthUrl);
            if (string.IsNullOrEmpty(responseText)) return false;
            return await _databaseController.SaveUser(responseText);
        }

        public async Task<bool> SaveAccount (AccountDummy dummyAccount)
        {
            var account = new Account(dummyAccount.Username,dummyAccount.Properties,dummyAccount.Cookies);
            var responseText = await VerifyAccount(account);
            return await _databaseController.SaveUser(responseText);
        }

        private async Task<string> VerifyAccount(Account account)
        {
            return await _oAuthController.VerifyAccount(Settings.OAuthUrl, account);
        }


        public void Logout ()
        {
            _oAuthController.DeAuthenticateSavedAccount();

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
			//return false;

		}

		private string GenerateDarkSkyWeatherRequestUri (string apiKey, double latitude, double longitude)
		{
			return string.Join (",", string.Join ("/", "https://api.darksky.net", "forecast", apiKey), latitude, longitude);
		}
	}
}
