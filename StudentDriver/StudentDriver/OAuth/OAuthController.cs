using System;
using System.Net;
using System.Threading.Tasks;
using OAuth.StudentDriver;
using Xamarin.Auth;

namespace OAuth
{
    public class OAuthController
    {
        public async Task<Response> MakePostRequest(string url, Account account)
        {
            return await MakeOAuthRequest("POST", url, account);
        }

        public async Task<string> VerifyAccount(string url, Account account)
        {
            if (account == null) return null;
            var response = await MakeOAuthRequest("POST", url, account);
            if (response?.StatusCode != HttpStatusCode.OK) return null;
            var responseText = response.GetResponseText();
            if (string.IsNullOrEmpty(responseText) || responseText.StartsWith("<")) return null;
            AccountHandler.SaveFacebookAccount(account);
            return responseText;
        }

        public async Task<string> VerifySavedAccount(string url)
        {

            return await VerifyAccount(url, AccountHandler.GetSavedFacebookAccount());
        }

        public void DeAuthenticateSavedAccount()
        {
            AccountHandler.DeAuthenticateAccount();
        }

        private async Task<Response> MakeOAuthRequest(string method, string url, Account account)
        {
            if (account == null) return null;
            return await new OAuthRequest(method, new Uri(url), null, account).GetResponseAsync();
        }
    }


}
