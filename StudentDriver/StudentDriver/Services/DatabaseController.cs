﻿using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudentDriver.Models;
using System;

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
			if (profileJson == null)
			{
				return false;
			}
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
			var newDriveId = await _database.StartUnsyncDrive();
			if (newDriveId == -1) return false;
			if (await AddWeatherToDrive(weatherJson, newDriveId))
			{
				return true;
			}
			await _database.DeleteUnsyncDriveById(newDriveId);
			return false;
		}

		public async Task<bool> StopCurrentUnsyncDrive()
		{
			var stoppedDrive = await _database.StopCurrentAsyncDrive();
			return (stoppedDrive != -1);
		}

		public async Task<bool> AddWeatherToDrive(string weatherJson, int unsyncDriveId)
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

		public async Task<DriveWeatherData> GetWeatherFromDrive(int unsyncDriveId)
		{
			return await _database.GetDriveWeatherByUnsyncDrive(unsyncDriveId);
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
			return await _database.DeleteUnsyncDriveById(driveId);
		}

		public async Task<int> CreateUnsyncDrive()
		{
			return await _database.StartUnsyncDrive();
		}

		public async Task<int> StopUnsyncDrive(int driveId)
		{
			return await _database.StopUnsyncDrive(driveId);
		}

		public async Task<DriveWeatherData> AddWeatherData(string weatherType, string weatherTemp, string weatherIcon, int unsyncDriveId)
		{
			var weatherObject = new DriveWeatherData
			{
				UnsyncDriveId = unsyncDriveId,
				WeatherType = weatherType,
				WeatherTemp = weatherTemp,
				WeatherIcon = weatherIcon,
			};
			if (await _database.AddDriveWeatherData(weatherObject) > -1)
			{
				return weatherObject;
			}
			return null;
		}

		public async Task<int> AddDrivePoints(IEnumerable<DrivePoint> list)
		{
			return await _database.AddDrivePoints(list);
		}

		public async Task<UnsyncDrive> GetUnsyncDriveById(int id)
		{
			return await _database.GetUnsyncDriveById(id);
		}

		public async Task<List<UnsyncDrive>> GetUnsyncedDrives()
		{
			return await _database.GetAllUnsyncedDrives();
		}

		public async Task<List<DrivePoint>> GetDrivePoints()
		{
			return await _database.GetAllDrivePoints();
		}

		public async Task<int> DeleteAllDriveData()
		{
			return await _database.DeleteAllDriveData();
		}


	}
}