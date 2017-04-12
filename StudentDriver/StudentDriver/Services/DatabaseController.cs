using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudentDriver.Models;

namespace StudentDriver.Services
{
    public class DatabaseController
    {
        public async Task<bool> SaveUser(string profileJson)
        {
            var jsonObj = JObject.Parse(profileJson);
            var firstName = jsonObj["_json"]["first_name"].ToString();
            var lastName = jsonObj["_json"]["last_name"].ToString();
            var imgUrl = jsonObj["photos"].First["value"].ToString();
            var user = await SQLiteDatabase.GetInstance().GetUser();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.ImageUrl = imgUrl;
            return await SQLiteDatabase.GetInstance().UpdateUser(user) != -1;
        }

        public async Task<bool> StartNewUnsyncDrive(string weatherJson)
        {
            if (string.IsNullOrEmpty(weatherJson)) return false;
            var newDriveId = await SQLiteDatabase.GetInstance().StartAsyncDrive();
            if (newDriveId == -1) return false;
            return await AddWeatherToDrive(weatherJson, newDriveId);
        }

        public async Task<bool> StopCurrentAsyncDrive()
        {
            var stoppedDrive = await SQLiteDatabase.GetInstance().StopCurrentAsyncDrive();
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
            return (await SQLiteDatabase.GetInstance().AddDriveWeatherData(weather) != -1);
        }

        public async Task<bool> ConnectStudentToDrivingSchool(string userJson)
        {
            var userObj = JObject.Parse(userJson);
            var schoolId = userObj["schoolId"].ToString();
            var user = await SQLiteDatabase.GetInstance().GetUser();
            user.DrivingSchoolId = schoolId;
            return (await SQLiteDatabase.GetInstance().UpdateUser(user) != -1);
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
