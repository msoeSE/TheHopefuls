using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using StudentDriver.Helpers;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		private SQLiteAsyncConnection _database;
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
;		    _database.CreateTableAsync<UnsyncDrive>();
	        _database.CreateTableAsync<WeatherData>();
		}

	    public List<DriveSession> GetUnsyncDriveSessions()
	    {
	        var driveSessions = new List<DriveSession>();
	        foreach (var unsyncDrive in _database.Table<UnsyncDrive>().ToListAsync().Result)
	        {
	            var drivePoints = _database.Table<DrivePoint>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).ToListAsync().Result;
	            var weatherData = _database.Table<WeatherData>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).FirstOrDefaultAsync().Result;
                driveSessions.Add(new DriveSession(unsyncDrive, drivePoints,weatherData));
            }
            return driveSessions;
	    }

	}
}
