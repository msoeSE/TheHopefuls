using System;
using System.Collections.Generic;
using SQLite;
using StudentDriver.Helpers;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		private readonly SQLiteAsyncConnection _database;
	    private static SQLiteDatabase _sqLiteDatabaseInstance;

	    public static SQLiteDatabase GetInstance()
	    {
	        return _sqLiteDatabaseInstance ?? (_sqLiteDatabaseInstance = new SQLiteDatabase());
	    }

	    private SQLiteDatabase ()
		{
			_database = DependencyService.Get<ISQLite>().GetAsyncConnection();
		    _database.CreateTableAsync<StateReqs>();
		    _database.CreateTableAsync<User>();
		    _database.CreateTableAsync<UserStats>();
	        _database.CreateTableAsync<DrivePoint>();
		    _database.CreateTableAsync<UnsyncDrive>();
	        _database.CreateTableAsync<DriveWeatherData>();
		}

	    public bool AddUser(User user)
	    {
	        return _database.InsertAsync(user).IsCompleted;
	    }

	    public bool UpdateStateReqs(IEnumerable<StateReqs> stateReqs)
	    {
	        return _database.UpdateAllAsync(stateReqs).IsCompleted;
	    }

	    public bool UpdateUserStats(UserStats userStats)
	    {
	        return _database.UpdateAsync(userStats).IsCompleted;
	    }

	    public User GetUser()
	    {
	        return _database.Table<User>().FirstOrDefaultAsync().Result;
	    }

	    public int StartAsyncDrive()
	    {
            var unsyncDrive = new UnsyncDrive
                              {
                                  UserId = GetUser().Id,
                                  StartDateTime = new DateTime().ToUniversalTime()
                              };
	        return _database.InsertAsync(unsyncDrive).Result;
	    }

	    public bool AddDrivePoint(DrivePoint drivePoint)
	    {
	        return _database.InsertAsync(drivePoint).IsCompleted;
	    }

        public bool AddDrivePoints(IEnumerable<DrivePoint> drivePoints)
        {
            return _database.InsertAllAsync(drivePoints).IsCompleted;
        }

	    public bool AddDriveWeatherData(DriveWeatherData driveWeatherData)
	    {
	        return _database.InsertAsync(driveWeatherData).IsCompleted;
	    }
        public void StopAsyncDrive(int unsyncDriveId)
	    {
	        var unsyncDrive = _database.Table<UnsyncDrive>().Where(x => x.Id == unsyncDriveId).FirstAsync().Result;
            unsyncDrive.EndDateTime = new DateTime().ToUniversalTime();
	        _database.UpdateAsync(unsyncDrive);
	    }

	    public List<DriveSession> GetUnsyncDriveSessions()
	    {
	        var driveSessions = new List<DriveSession>();
	        foreach (var unsyncDrive in _database.Table<UnsyncDrive>().ToListAsync().Result)
	        {
	            var drivePoints = _database.Table<DrivePoint>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).ToListAsync().Result;
	            var weatherData = _database.Table<DriveWeatherData>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).FirstOrDefaultAsync().Result;
                driveSessions.Add(new DriveSession(unsyncDrive, drivePoints,weatherData));
            }
            return driveSessions;
	    }

	}
}
