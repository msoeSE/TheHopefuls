using System;
using System.Threading.Tasks;
using OAuth.StudentDriver;
using Xamarin.Auth;

namespace OAuth
{
    public class OAuthController
    {

        public OAuthController()
        {
            OAuthSettings.InitializeKeys();
        }

        public async Task<Response> MakePostRequest(string url,Account account)
        {
            return await MakeOAuthRequest("POST",url, account);
        }

        public async Task<Response> GetProfile(Account account)
        {
            return await MakeOAuthRequest("GET", OAuthSettings.FACEBOOK_PROFILE_REQUEST_URL, account);
        }

        private async Task<Response> MakeOAuthRequest(string method, string url, Account account)
        {
            if (account == null) return null;
            return await new OAuthRequest(method, new Uri(url), null, account).GetResponseAsync();
        }
    }


}
