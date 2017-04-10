using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
