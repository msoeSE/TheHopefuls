using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace OAuth
{
    public interface IOAuthController
    {
        void DeAuthenticateSavedAccount();
        Task<string> MakeGetRequest(string url, IDictionary<string, string> parameters = null);
        Task<string> MakePostRequest(string url, IDictionary<string, string> parameters = null);
        Task<string> VerifyAccount(string url, Account account);
        Task<string> VerifySavedAccount(string url);
    }
}