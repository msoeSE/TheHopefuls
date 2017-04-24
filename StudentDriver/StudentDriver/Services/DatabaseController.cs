using System.Threading.Tasks;
using Newtonsoft.Json;
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudentDriver.Models;

namespace StudentDriver.Services
{
    public class DatabaseController : IDatabaseController
    {
        private readonly SQLiteDatabase _database;

        public DatabaseController()
        {
            _database = new SQLiteDatabase();
        }

        public async Task<bool> SaveUser(string profileJson)
        {
            var jsonObj = JObject.Parse(profileJson);
            var mongoId = jsonObj["mongoID"].ToString();
            var firstName = jsonObj["_json"]["first_name"].ToString();
            var lastName = jsonObj["_json"]["last_name"].ToString();
            var imgUrl = jsonObj["photos"].First["value"].ToString();
            var userTypeStr = jsonObj["userType"].ToString();
            //var userTypeStr = "instructor";
            var userType = User.UserType.Student;
            if (userTypeStr.Equals("instructor"))
            {
                userType = User.UserType.Instructor;
            }
            var user = await _database.GetUser();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.ImageUrl = imgUrl;
            user.UType = userType;
            user.ServerId = mongoId;
            return await _database.UpdateUser(user) != -1;
        }

        public async Task<bool> StartNewUnsyncDrive(string weatherJson)
        {
            if (string.IsNullOrEmpty(weatherJson)) return false;
            var newDriveId = await _database.StartAsyncDrive();
            if (newDriveId == -1) return false;
            return await AddWeatherToDrive(weatherJson, newDriveId);
        }

        public async Task<bool> StopCurrentAsyncDrive()
        {
            var stoppedDrive = await _database.StopCurrentAsyncDrive();
            return (stoppedDrive != -1);
        }

        private async Task<bool> AddWeatherToDrive(string weatherJson, int unsyncDriveId)
        {
            var jsonObj = JObject.Parse(weatherJson);
            var weatherType = jsonObj["weatherType"].ToString();
            var timeOfDay = jsonObj["timeDay"].ToString();

            var weather = new DriveWeatherData
                          {
                              UnsyncDriveId = unsyncDriveId,
                              WeatherType = weatherType,
                              TimeOfDay = timeOfDay
                          };
            return (await _database.AddDriveWeatherData(weather) != -1);
        }

        public async Task<bool> ConnectStudentToDrivingSchool(string userJson)
        {
            var userObj = JObject.Parse(userJson);
            var schoolId = userObj["schoolId"].ToString();
            var user = await _database.GetUser();
            user.DrivingSchoolId = schoolId;
            return (await _database.UpdateUser(user) != -1);
        }

        public async Task<StateReqs> GetStateRequirements(string state)
        {
            return await _database.GetStateReqs(state);
        }

        public async Task<bool> StoreStateRequirements(string stateReqJson)
        {
            var stateReq = JsonConvert.DeserializeObject<StateReqs>(stateReqJson);
            return (await _database.AddStateReqs(stateReq) != -1);
        }

        public async Task<User> GetUser()
        {
            var user = await _database.GetUser();
            return user;
        }

		public async Task<bool> DeleteUnsyncDrive(int driveId)
		{
			return await SQLiteDatabase.GetInstance().DeleteUnsyncDriveById(driveId);
		}

		public async Task<int> CreateUnsyncDrive()
		{
			return await SQLiteDatabase.GetInstance().StartUnsyncDrive();
		}

		public async Task<int> StopUnsyncDrive(int driveId)
		{
			return await SQLiteDatabase.GetInstance().StopUnsyncDrive(driveId);
		}

		public async Task<int> AddDrivePoints(IEnumerable<DrivePoint> list)
		{
			return await SQLiteDatabase.GetInstance().AddDrivePoints(list);
		}

		public async Task<UnsyncDrive> GetUnsyncDriveById(int id)
		{
			return await SQLiteDatabase.GetInstance().GetUnsyncDriveById(id);
		}

		public async Task<List<UnsyncDrive>> GetUnsyncedDrives()
		{
			return await SQLiteDatabase.GetInstance().GetAllUnsyncedDrives();
		}

		public async Task<List<DrivePoint>> GetDrivePoints()
		{
			return await SQLiteDatabase.GetInstance().GetAllDrivePoints();
		}

    }
}
