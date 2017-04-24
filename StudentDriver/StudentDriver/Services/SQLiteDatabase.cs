using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SQLite.Net.Async;
using StudentDriver.Helpers;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		private static SQLiteAsyncConnection _database;

        public SQLiteDatabase()
        { 
            _database = DependencyService.Get<ISQLite>().GetAsyncConnection();
            _database.CreateTableAsync<StateReqs>().Wait();
            _database.CreateTableAsync<UserStats>().Wait();
            _database.CreateTableAsync<DrivePoint>().Wait();
            _database.CreateTableAsync<UnsyncDrive>().Wait();
            _database.CreateTableAsync<DriveWeatherData>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

	    public void DeleteDatabase()
	    {
	        try
	        {
	            _database.DropTableAsync<StateReqs>().Wait();
	            _database.DropTableAsync<UserStats>().Wait();
	            _database.DropTableAsync<DrivePoint>().Wait();
	            _database.DropTableAsync<UnsyncDrive>().Wait();
	            _database.DropTableAsync<DriveWeatherData>().Wait();
	            _database.DropTableAsync<User>().Wait();
                DependencyService.Get<ISQLite>().DeleteDatabase();
	        }
	        catch (Exception e)
	        {
	            var d = e;
	        }

        }


	    public async Task<User> AddUser(User user)
	    {
	        try
	        {
                await _database.InsertAsync(user);
            }
	        catch (Exception e)
	        {
	            throw;
	        }
	        return await GetUser();
	    }

        public async Task<User> GetUser()
        {
            User user = null;
            try
            {
                user = _database.Table<User>().FirstOrDefaultAsync().Result;
                if (user == null)
                {
                    user = await AddUser(new User());
                }
            }
            catch (Exception ex)
            {
                var d = ex;
            }
            return user;
        }

        public async Task<int> UpdateUser(User user)
        {
            int result = -1;
            try
            {
                result = await _database.UpdateAsync(user).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

	    public async Task<int> AddStateReqs(StateReqs stateReqs)
	    {
	        return await _database.InsertAsync(stateReqs);
	    }

	    public async Task<StateReqs> GetStateReqs(string state)
	    {
	        return await _database.Table<StateReqs>().Where(x => x.State == state).FirstOrDefaultAsync();
	    }

        public async Task<int> UpdateStateReqs(IEnumerable<StateReqs> stateReqs)
        {
            return await _database.UpdateAllAsync(stateReqs);
        }

        public async Task<int> UpdateUserStats(UserStats userStats)
        {
            return await _database.UpdateAsync(userStats);
        }

        public async Task<int> StartAsyncDrive()
        {
            var unsyncDrive = new UnsyncDrive
            {
                UserId = GetUser().Id,
                StartDateTime = new DateTime().ToUniversalTime()
            };
            return await _database.InsertAsync(unsyncDrive);
        }

        public async Task<int> StopCurrentAsyncDrive()
        {
            var unsyncDrive = _database.Table<UnsyncDrive>().OrderByDescending(u => u.Id).FirstOrDefaultAsync().Result;
            unsyncDrive.EndDateTime = new DateTime().ToUniversalTime();
            return await _database.UpdateAsync(unsyncDrive);
        }


        public async Task<int> AddDrivePoint(DrivePoint drivePoint)
        {
            return await _database.InsertAsync(drivePoint);
        }

        public async Task<int> AddDrivePoints(IEnumerable<DrivePoint> drivePoints)
        {
            return await _database.InsertAllAsync(drivePoints);
        }

        public async Task<int> AddDriveWeatherData(DriveWeatherData driveWeatherData)
        {
            return await _database.InsertAsync(driveWeatherData);
        }


        public async Task<List<DriveSession>> GetUnsyncDriveSessions()
        {
            var driveSessions = new List<DriveSession>();
            foreach (var unsyncDrive in await _database.Table<UnsyncDrive>().ToListAsync())
            {
                var drivePoints = _database.Table<DrivePoint>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).ToListAsync().Result;
                var weatherData = _database.Table<DriveWeatherData>().Where(x => x.UnsyncDriveId == unsyncDrive.Id).FirstOrDefaultAsync().Result;
                driveSessions.Add(new DriveSession(unsyncDrive, drivePoints, weatherData));
            }
            return driveSessions;
        }

    }
}
