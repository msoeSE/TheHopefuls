using System.Threading.Tasks;
using StudentDriver.Models;

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
    }
}