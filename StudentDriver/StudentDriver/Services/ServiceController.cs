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
    public class ServiceController
    {
        private static ServiceController _instance;
        private readonly OAuthController _oAuthController;
        private readonly DatabaseController _databaseController;


        public static ServiceController Instance => _instance ?? (_instance = new ServiceController());


        private ServiceController()
        {
            _oAuthController = new OAuthController();
            _databaseController = new DatabaseController();
        }

        //public async Task<UserStats> GetStudentStats(int id)
        //{
        //    var requestUri = GenerateRequestUri(_apiBaseUrl, "students", new Dictionary<string, string>() { { "userId", id.ToString() } });
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

        public async Task<bool> UserLoggedIn()
        {
            var responseText = await _oAuthController.VerifySavedAccount(Settings.OAuthUrl);
            if (string.IsNullOrEmpty(responseText)) return false;
            var loggedIn = await _databaseController.SaveUser(responseText);
            return loggedIn;
        }

        public async Task<bool> SaveAccount (AccountDummy dummyAccount)
        {
            var account = new Account(dummyAccount.Username,dummyAccount.Properties,dummyAccount.Cookies);
            var responseText = await VerifyAccount(account);
            return await _databaseController.SaveUser(responseText);
        }

        public async Task<bool> ConnectSchool(string schoolId)
        {
            var parameters = new Dictionary<string,string>
                {
                    {"schoolId", schoolId}
                };
            var response =  await _oAuthController.MakePostRequest(Settings.SchoolIdUrl, parameters);
            var responseText = response.GetResponseText();
            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(responseText)) return false;
            return await _databaseController.ConnectStudentToDrivingSchool(responseText);
        }

        public async Task<bool> StartUnsyncDrive(double latitude, double longitude)
        {
            var weather = await GetWeather(latitude, longitude);
            return await _databaseController.StartNewUnsyncDrive(weather);
        }

        public async Task<DrivingDataViewModel> GetAggregatedDrivingData(string state, string userId = null)
        {
            Dictionary<string, string> parameters = null;
            if (!string.IsNullOrEmpty(userId))
            {
                parameters = new Dictionary<string, string>
                {
                    { "userId", userId}
                };
            }
            var response = await _oAuthController.MakeGetRequest(Settings.AggregateDrivingUrl, parameters);
            var responseText = response.GetResponseText();
            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(responseText)) return null;

            var aggData = JsonConvert.DeserializeObject<DrivingAggregateData>(responseText);
            var stateReq = await GetStateRequirements(state);
            return new DrivingDataViewModel(stateReq, aggData);
        }

        private async Task<string> GetWeather(double latitude, double longitude)
        {
            var parameters = new Dictionary<string, string>
                             {
                                {"longitude", longitude.ToString()},
                                {"latitude", latitude.ToString()},
                             };
            var response = await _oAuthController.MakeGetRequest(Settings.WeatherUrl, parameters);
            var responseText = response.GetResponseText();
            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(responseText)) return null;
            return responseText;

        }

        private async Task<StateReqs> GetStateRequirements(string state)
        {
            var stateReq = await _databaseController.GetStateRequirements(state);
            if (stateReq != null) return stateReq;
            var statReqJson = await GetRequestStateRequirements(state);
            var storeSuccessful = await _databaseController.StoreStateRequirements(statReqJson);
            if (!storeSuccessful) return null;
            return await _databaseController.GetStateRequirements(state);
        }

        private async Task<string> GetRequestStateRequirements(string state)
        {
            var parameters = new Dictionary<string, string>
            {
                { "state",state}
            };
            var response = await _oAuthController.MakeGetRequest(Settings.StateReqUrl, parameters);
            var responseText = response.GetResponseText();
            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(responseText)) return null;
            return responseText;
        }



        private async Task<string> VerifyAccount(Account account)
        {
            return await _oAuthController.VerifyAccount(Settings.OAuthUrl, account);
        }


        public void Logout ()
        {
            _oAuthController.DeAuthenticateSavedAccount();
		}
	}
}
