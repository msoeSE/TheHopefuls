using System.Collections.Generic;
using System.Threading.Tasks;
using StudentDriver.Helpers;
using StudentDriver.Models;

namespace StudentDriver
{
    public interface ISQLiteDatabase
    {
        Task<int> AddDrivePoint(DrivePoint drivePoint);
        Task<int> AddDrivePoints(IEnumerable<DrivePoint> drivePoints);
        Task<int> AddDriveWeatherData(DriveWeatherData driveWeatherData);
        Task<int> AddStateReqs(StateReqs stateReqs);
        Task<User> AddUser(User user);
        void DeleteDatabase();
        Task<StateReqs> GetStateReqs(string state);
        Task<List<DriveSession>> GetUnsyncDriveSessions();
        Task<User> GetUser();
        Task<int> StartUnsyncDrive();
        Task<int> StopCurrentAsyncDrive();
        Task<int> UpdateStateReqs(IEnumerable<StateReqs> stateReqs);
        Task<int> UpdateUser(User user);
        Task<int> UpdateUserStats(UserStats userStats);
        Task<bool> DeleteUnsyncDriveById(int id);
        Task<DriveWeatherData> GetDriveWeatherByUnsyncDrive(int unsyncDriveId);
        Task<int> StopUnsyncDrive(int unsyncDriveId);
        Task<UnsyncDrive> GetUnsyncDriveById(int id);
        Task<List<UnsyncDrive>> GetAllUnsyncedDrives();
        Task<List<DrivePoint>> GetAllDrivePoints();
        Task<int> DeleteAllDriveData();
    }
}