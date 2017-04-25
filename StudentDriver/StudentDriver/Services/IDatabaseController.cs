using System.Threading.Tasks;
using StudentDriver.Models;
using System.Collections.Generic;

namespace StudentDriver.Services
{
	public interface IDatabaseController
	{
		Task<bool> ConnectStudentToDrivingSchool(string userJson);
		Task<StateReqs> GetStateRequirements(string state);
		Task<User> GetUser();
		Task<bool> SaveUser(string profileJson);
		Task<bool> StartNewUnsyncDrive(string weatherJson);
		Task<bool> StopCurrentAsyncDrive();
		Task<bool> StoreStateRequirements(string stateReqJson);
		Task<List<DrivePoint>> GetDrivePoints();
		Task<List<UnsyncDrive>> GetUnsyncedDrives();
		Task<int> AddDrivePoints(IEnumerable<DrivePoint> list);
		Task<UnsyncDrive> GetUnsyncDriveById(int id);
		Task<int> StopUnsyncDrive(int driveId);
		Task<bool> DeleteUnsyncDrive(int driveId);
		Task<int> CreateUnsyncDrive();
		Task<DriveWeatherData> GetWeatherFromDrive(int unsyncDriveId)
Task AddWeatherData(string weatherType, string weatherTemp, string weatherIcon, int unsyncDriveId);
	}
}