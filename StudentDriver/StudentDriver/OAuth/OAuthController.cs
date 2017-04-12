using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OAuth.StudentDriver;
using Xamarin.Auth;

namespace OAuth
{
    public class OAuthController
    {
        public async Task<Response> MakeGetRequest(string url, IDictionary<string, string> parameters = null)
        {
            return await MakeOAuthRequest(OAuthRequest.Get, url, AccountHandler.GetSavedFacebookAccount(), parameters);
        }

        public async Task<Response> MakePostRequest(string url, JObject jsonJObject)
        {
            return await MakeOAuthRequest(OAuthRequest.Post, url, AccountHandler.GetSavedFacebookAccount(), null, jsonJObject.ToString());
        }

        public async Task<string> VerifyAccount(string url, Account account)
        {
            if (account == null) return null;
            var response = await MakeOAuthRequest(OAuthRequest.Post, url, account);
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

        private static async Task<Response> MakeOAuthRequest(string method,string url, Account account,IDictionary<string, string> parameters = null, string jsonBody = null)
        {
            if (account == null) return null;
            return await new OAuthRequest(method, new Uri(url), parameters, account,jsonBody).GetResponseAsync();
        }
    }


}
