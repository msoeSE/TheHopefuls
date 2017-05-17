using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SQLite.Net.Async;
using StudentDriver.Helpers;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase : ISQLiteDatabase
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
			await _database.InsertAsync(user);
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
			int result;
			result = await _database.UpdateAsync(user).ConfigureAwait(false);
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

		public async Task<int> StartUnsyncDrive()
		{
			var startTime = DateTime.Now;
			var userId = GetUser().Id;
			var unsyncDrive = new UnsyncDrive
			{
				UserId = userId,
				StartDateTime = startTime,
			};

			//TODO This returns amount of fields inserted, not the ID.
			var inserted = await _database.InsertAsync(unsyncDrive);
			if (inserted > 0)
			{
				var drive = await _database.Table<UnsyncDrive>().Where(x => x.UserId == userId && x.EndDateTime == null)
										   .OrderByDescending(x => x.Id).FirstOrDefaultAsync();
				if (drive != null)
				{
					return drive.Id;
				}
			}
			return -1;
		}

		//public async Task<int> StopCurrentAsyncDrive()
		//{

		//	var unsyncDrive = await _database.Table<UnsyncDrive>().OrderByDescending(u => u.Id).FirstOrDefaultAsync();
		//	unsyncDrive.EndDateTime = DateTime.Now;
		//	return await _database.UpdateAsync(unsyncDrive);
		//}


		public async Task<int> AddDrivePoint(DrivePoint drivePoint)
		{
			return await _database.InsertAsync(drivePoint);
		}

		public async Task<int> AddDrivePoints(IEnumerable<DrivePoint> drivePoints)
		{
			return await _database.InsertAllAsync(drivePoints);
		}

		public async Task<List<DrivePoint>> GetAllDrivePoints()
		{
			return await _database.Table<DrivePoint>().ToListAsync();
		}

		public async Task<List<UnsyncDrive>> GetAllUnsyncedDrives()
		{
			return await _database.Table<UnsyncDrive>().Where(x => x.EndDateTime != null).ToListAsync();
		}

		public async Task<UnsyncDrive> GetUnsyncDriveById(int id)
		{
			return await _database.Table<UnsyncDrive>().Where(x => x.Id == id).FirstAsync();
		}

		public async Task<bool> DeleteUnsyncDriveById(int id)
		{
			var drive = await _database.Table<UnsyncDrive>().Where(x => x.Id == id).FirstAsync();
			if (drive != null)
			{
				await _database.DeleteAsync(drive);
				return true;
			}
			return false;
		}

		public async Task<int> DeleteAllDriveData()
		{
			int total = await _database.DeleteAllAsync<DrivePoint>();
			total += await _database.DeleteAllAsync<UnsyncDrive>();
			total += await _database.DeleteAllAsync<DriveWeatherData>();
			return total;
		}

		public async Task<int> AddDriveWeatherData(DriveWeatherData driveWeatherData)
		{
			return await _database.InsertAsync(driveWeatherData);
		}

		public async Task<DriveWeatherData> GetDriveWeatherByUnsyncDrive(int unsyncDriveId)
		{
			try
			{
				return await _database.Table<DriveWeatherData>().Where(x => x.UnsyncDriveId == unsyncDriveId).FirstAsync();
			}
			catch (Exception)
			{
				return null;
			}

		}

		public async Task<int> StopUnsyncDrive(int unsyncDriveId)
		{
			Debug.WriteLine(await _database.Table<UnsyncDrive>().CountAsync());
			var unsyncDrive = await _database.Table<UnsyncDrive>().Where(x => x.Id == unsyncDriveId).FirstOrDefaultAsync();
			if (unsyncDrive != null)
			{
				unsyncDrive.EndDateTime = DateTime.UtcNow;
				return await _database.UpdateAsync(unsyncDrive);
			}
			return -1;

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
