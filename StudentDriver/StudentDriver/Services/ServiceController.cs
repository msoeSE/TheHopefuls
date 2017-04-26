using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using StudentDriver.Models;
using Newtonsoft.Json;
using OAuth;
using OAuthAccess;
using StudentDriver.Helpers;
using Xamarin.Auth;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

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
		public async Task<bool> SaveAccount(AccountDummy dummyAccount)
		{
			var account = new Account(dummyAccount.Username, dummyAccount.Properties, dummyAccount.Cookies);
			var responseText = await VerifyAccount(account);
			return await _databaseController.SaveUser(responseText);
		}

		public async Task<bool> PostDrivePoints(List<DrivePoint> drivePoints, List<UnsyncDrive> unsyncDrives)
		{
			var user = await App.ServiceController.GetUser();
			var endpoint = string.Format(Settings.FORMATStudentDrivingSessionsUrl, user.ServerId);
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);
			try
			{
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					writer.Formatting = Formatting.Indented;
					writer.WriteStartArray();
					foreach (var drive in unsyncDrives)
					{
						var driveWeather = await _databaseController.GetWeatherFromDrive(drive.Id);
						if (driveWeather == null)
						{
							continue;
						}
						writer.WriteStartObject();
						writer.WritePropertyName("UnsyncDrive");
						writer.WriteStartObject();
						writer.WritePropertyName("startTime");
						writer.WriteValue(drive.StartDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm+ss:FFZ"));
						writer.WritePropertyName("endTime");
						writer.WriteValue(drive.EndDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm+ss:FFZ"));
						writer.WriteEndObject();
						writer.WritePropertyName("DrivePoints");
						writer.WriteStartArray();
						var validDrivePoints = drivePoints.FindAll(x => x.UnsyncDriveId == drive.Id);
						foreach (var drivePoint in validDrivePoints)
						{
							writer.WriteStartObject();
							writer.WritePropertyName("time");
							writer.WriteValue(drivePoint.PointDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm+ss:FFZ"));
							writer.WritePropertyName("lat");
							writer.WriteValue(drivePoint.Latitude);
							writer.WritePropertyName("lon");
							writer.WriteValue(drivePoint.Longitude);
							writer.WritePropertyName("speed");
							writer.WriteValue(drivePoint.Speed);
							writer.WriteEndObject();
						}
						writer.WriteEndArray();
						writer.WritePropertyName("DriveWeatherData");
						writer.WriteStartObject();
						writer.WritePropertyName("temperature");
						writer.WriteValue(driveWeather.WeatherTemp);
						writer.WritePropertyName("summary");
						writer.WriteValue(driveWeather.WeatherType);
						writer.WriteEndObject();
						//End the drivepoint object
						writer.WriteEndObject();
					}
					writer.WriteEndArray();
					writer.ToString();
				}
				var jsonString = sb.ToString();
				var response = await _oAuthController.MakePostRequest(endpoint); ;
				return response.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception e)
			{
				return false;
			}

			return false;
		}

		public async Task<bool> DeleteAllDriveData()
		{
			return await _databaseController.DeleteAllDriveData() > 0;
		}

		public async Task<List<DrivePoint>> GetAllDrivePoints()
		{
			return await _databaseController.GetDrivePoints();
		}
		public async Task<List<UnsyncDrive>> GetAllUnsyncedDrives()
		{
			return await _databaseController.GetUnsyncedDrives();
		}

		public async Task<bool> ConnectSchool(string schoolId)
		{
			var parameters = new Dictionary<string, string>
				{
					{"schoolId", schoolId}
				};
			var response = await _oAuthController.MakePostRequest(Settings.SchoolIdUrl, parameters);
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

		public async Task<string> GetWeather(double latitude, double longitude)
		{
			var urlForRequest = string.Format("{0}/{1}/{2}", Settings.WeatherUrl, latitude, longitude);
			var response = await _oAuthController.MakeGetRequest(urlForRequest);
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



		public async Task<DriveWeatherData> CreateDriveWeatherData(double latitude, double longitude, int unsyncDriveId)
		{
			var responseString = await this.GetWeather(latitude, longitude);
			try
			{
				JToken token = JObject.Parse(responseString);
				var weatherType = (string)token.SelectToken("summary");
				var weatherIcon = (string)token.SelectToken("icon");
				var weatherTemp = (string)token.SelectToken("temperature");
				return await _databaseController.AddWeatherData(weatherType, weatherTemp, weatherIcon, unsyncDriveId);
			}
			catch (Exception e)
			{
				return null;
			}

			return null;

		}

		public async Task<bool> DeleteUnsyncDrive(int driveId)
		{
			return await _databaseController.DeleteUnsyncDrive(driveId);
		}

		private async Task<string> VerifyAccount(Account account)
		{
			return await _oAuthController.VerifyAccount(Settings.OAuthUrl, account);
		}
		public async Task<int> CreateUnsyncDrive()
		{
			return await _databaseController.CreateUnsyncDrive();
		}

		public async Task<UnsyncDrive> GetUnsyncedDriveById(int id)
		{
			return await _databaseController.GetUnsyncDriveById(id);
		}

		public async Task<int> StopUnsyncDrive(int driveId)
		{
			return await _databaseController.StopUnsyncDrive(driveId);
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

		public async Task<int> AddDrivePoints(IEnumerable<DrivePoint> list)
		{
			return await _databaseController.AddDrivePoints(list);
		}

		public void Logout()
		{
			_oAuthController.DeAuthenticateSavedAccount();
		}
	}
}
