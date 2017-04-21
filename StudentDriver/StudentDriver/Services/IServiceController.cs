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
    }
}