using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDriver.Models;

namespace StudentDriver.Services
{
    public class DatabaseController : IDatabaseController
    {
        private readonly ISQLiteDatabase _database;

        public DatabaseController(ISQLiteDatabase database)
        {
            _database = database;
        }

        public async Task<bool> SaveUser(string profileJson)
        {
            if (string.IsNullOrEmpty(profileJson)) return false;
            var jsonObj = JObject.Parse(profileJson);
            string mongoId;
            string firstName;
            string lastName;
            string imgUrl;
            string userTypeStr;
            try
            {
                mongoId = jsonObj["mongoID"].ToString();
                firstName = jsonObj["_json"]["first_name"].ToString();
                lastName = jsonObj["_json"]["last_name"].ToString();
                imgUrl = jsonObj["photos"].First["value"].ToString();
                userTypeStr = jsonObj["userType"].ToString();
            }
            catch (NullReferenceException e)
            {
                return false;
            }
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
            //user.UType = User.UserType.Instructor;
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
            if (string.IsNullOrEmpty(userJson)) return false;
            var userObj = JObject.Parse(userJson);
            string schoolId;
            try
            {
                schoolId = userObj["schoolId"].ToString();
            }
            catch (NullReferenceException e)
            {
                return false;
            }
            var user = await _database.GetUser();
            user.DrivingSchoolId = schoolId;
            return (await _database.UpdateUser(user) != -1);
        }

        public async Task<StateReqs> GetStateRequirements(string state)
        {
            if (string.IsNullOrEmpty(state)) return null;
            return await _database.GetStateReqs(state);
        }

        public async Task<bool> StoreStateRequirements(string stateReqJson)
        {
            if (string.IsNullOrEmpty(stateReqJson)) return false;
            var stateReq = JsonConvert.DeserializeObject<StateReqs>(stateReqJson);
            return (await _database.AddStateReqs(stateReq) != -1);
        }

        public async Task<User> GetUser()
        {
            var user = await _database.GetUser();
            return user;
        }
    }
}
