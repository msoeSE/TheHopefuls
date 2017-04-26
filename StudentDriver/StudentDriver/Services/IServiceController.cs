using System.Collections.Generic;
using System.Threading.Tasks;
using OAuthAccess;
using StudentDriver.Helpers;
using StudentDriver.Models;

namespace StudentDriver.Services
{
	public interface IServiceController
	{
		Task<bool> ConnectSchool(string schoolId);
		Task<DrivingDataViewModel> GetAggregatedDrivingData(string state, string userId = null);
		Task<User> GetUser();
		void Logout();
		Task<bool> SaveAccount(AccountDummy dummyAccount);
		Task<bool> StartUnsyncDrive(double latitude, double longitude);
		Task<bool> UserLoggedIn();
		Task<IEnumerable<User>> GetStudents();
		Task<int> AddDrivePoints(IEnumerable<DrivePoint> list);
		Task<bool> PostDrivePoints(List<DrivePoint> drivePoints, List<UnsyncDrive> unsyncDrives);
		Task<List<DrivePoint>> GetAllDrivePoints();
		Task<List<UnsyncDrive>> GetAllUnsyncedDrives();
		Task<int> CreateUnsyncDrive();
		Task<DriveWeatherData> CreateDriveWeatherData(double latitude, double longitude, int unsyncDriveId);
		Task<int> StopUnsyncDrive(int driveId);
		Task<string> GetWeather(double latitude, double longitude);
		Task<bool> DeleteAllDriveData();
		Task<bool> DeleteUnsyncDrive(int driveId);
	}
}