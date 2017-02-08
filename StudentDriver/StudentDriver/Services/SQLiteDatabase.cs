using System;
using System.Threading;
using System.Threading.Tasks;
using SQLite.Net.Async;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		private static SQLiteAsyncConnection _database;
	    private static SQLiteDatabase _sqLiteDatabaseInstance;

        public static SQLiteDatabase GetInstance()
	    {
	        return _sqLiteDatabaseInstance ?? (_sqLiteDatabaseInstance = new SQLiteDatabase());
	    }

        private SQLiteDatabase()
        { 
            _database = DependencyService.Get<ISQLite>().GetAsyncConnection();
            _database.CreateTableAsync<StateReqs>().Wait();
            _database.CreateTableAsync<UserStats>().Wait();
            _database.CreateTableAsync<DrivePoint>().Wait();
            _database.CreateTableAsync<UnsyncDrive>().Wait();
            _database.CreateTableAsync<DriveWeatherData>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }


	    public async Task<User> AddUser(User user)
	    {
	        try
	        {
                await _database.InsertAsync(user);
            }
	        catch (Exception e)
	        {
	            var d = e;
	        }
	        return await GetUser();
	    }

        public async Task<int> UpdateUser(User user)
        {
            int result = -1;
            try
            {
                var currentUser = GetUser().Result;
                if (currentUser.ImageUrl != user.ImageUrl)
                {
                    
                }
                result =  await _database.UpdateAsync(user);
            }
            catch (Exception e)
            {
                var d = e;
            }
            return result;
        }

        //public async Task<int> UpdateStateReqs(IEnumerable<StateReqs> stateReqs)
        //{
        //    return await _database.UpdateAllAsync(stateReqs);
        //}

        //public async Task<int> UpdateUserStats(UserStats userStats)
        //{
        //    return await _database.UpdateAsync(userStats);
        //}

        public async Task<User> GetUser()
        {
            User user = null;
            try
            {
                user = await _database.Table<User>().FirstOrDefaultAsync();
                if (user == null)
                {
                    user =  await AddUser(new User());
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }
            return user;
        }

	    //public async Task<int> StartAsyncDrive()
	    //{
     //       var unsyncDrive = new UnsyncDrive
     //                         {
     //                             UserId = GetUser().Id,
     //                             StartDateTime = new DateTime().ToUniversalTime()
     //                         };
	    //    return await _database.InsertAsync(unsyncDrive);
	    //}

	    //public async Task<int> AddDrivePoint(DrivePoint drivePoint)
	    //{
	    //    return await _database.InsertAsync(drivePoint);
	    //}

     //   public async Task<int> AddDrivePoints(IEnumerable<DrivePoint> drivePoints)
     //   {
     //       return await _database.InsertAllAsync(drivePoints);
     //   }

	    //public async Task<int> AddDriveWeatherData(DriveWeatherData driveWeatherData)
	    //{
	    //    return await _database.InsertAsync(driveWeatherData);
	    //}
     //   public async Task<int> StopAsyncDrive(int unsyncDriveId)
	    //{
	    //    var unsyncDrive = _database.Table<UnsyncDrive>().Where(x => x.Id == unsyncDriveId).FirstAsync().Result;
     //       unsyncDrive.EndDateTime = new DateTime().ToUniversalTime();
	    //    return await _database.UpdateAsync(unsyncDrive);
	    //}

	    //public async Task<List<DriveSession>> GetUnsyncDriveSessions()
	    //{
	    //    var driveSessions = new List<DriveSession>();
	    //    foreach (var unsyncDrive in await _database.Table<UnsyncDrive>().ToListAsync())
	    //    {
	    //        var drivePoints = _database.Table<DrivePoint>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).ToListAsync().Result;
	    //        var weatherData = _database.Table<DriveWeatherData>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).FirstOrDefaultAsync().Result;
     //           driveSessions.Add(new DriveSession(unsyncDrive, drivePoints,weatherData));
     //       }
     //       return driveSessions;
	    //}

	}
}
