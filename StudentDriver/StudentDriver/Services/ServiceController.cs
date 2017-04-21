using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using StudentDriver.Models;
using Newtonsoft.Json;
using OAuth;
using OAuthAccess;
using StudentDriver.Helpers;
using Xamarin.Auth;

namespace StudentDriver.Services
{
    public class ServiceController : IServiceController
    {
        private readonly IOAuthController _oAuthController;
        private readonly IDatabaseController _databaseController;

        public ServiceController(IOAuthController oAuthController, IDatabaseController databaseController)
        {
            _oAuthController = oAuthController;
            _databaseController = databaseController;
        }

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
            var responseText = response?.GetResponseText();
            if (response?.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(responseText)) return null;
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

        public async Task<User> GetUser()
        {
           return await _databaseController.GetUser();
        }

        public async Task<IEnumerable<User>> GetStudents()
        {
            var response = await _oAuthController.MakeGetRequest(Settings.InstructorStudentsUrl);
            var responseText = response.GetResponseText();
            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(responseText)) return null;
            var students = JsonConvert.DeserializeObject<IEnumerable<User>>(responseText);
            return students;
        }
    }
}
