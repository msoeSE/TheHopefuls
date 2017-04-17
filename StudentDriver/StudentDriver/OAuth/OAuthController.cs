using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OAuth.StudentDriver;
using StudentDriver.Helpers;
using Xamarin.Auth;

namespace OAuth
{
    public class OAuthController
    {
        public async Task<DummyResponse> MakeGetRequest(string url, IDictionary<string, string> parameters = null)
        {
            return await MakeOAuthRequest(DummyRequest.Get, url, AccountHandler.GetSavedFacebookAccount(), parameters);
        }

        public async Task<DummyResponse> MakePostRequest(string url, IDictionary<string, string> parameters = null)
        {
            return await MakeOAuthRequest(DummyRequest.Post, url, AccountHandler.GetSavedFacebookAccount(),parameters);
        }

        public async Task<string> VerifyAccount(string url, Account account)
        {
            if (account == null) return null;
            var response = await MakeOAuthRequest(DummyRequest.Post, url, account);
            if (response?.StatusCode != HttpStatusCode.OK) return null;
            var responseText = response.GetResponseText();
            if (string.IsNullOrEmpty(responseText)) return null;
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

        private static async Task<DummyResponse> MakeOAuthRequest(string method,string url, Account account,IDictionary<string, string> parameters = null)
        {
            if (account == null) return null;
            var request = new DummyRequest(method,new Uri(url),account,parameters);
            return await request.GetResponseAsync(CancellationToken.None);
        }
    }


}
