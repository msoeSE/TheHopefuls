using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xamarin.Auth;
using Xamarin.Utilities;

namespace OAuth
{
    public class OAuthRequest: Request
    {
        public static string Get = "GET";
        public static string Post = "POST";
        private const string AuthtenticationHeaderKey = "access_token";
        

        public OAuthRequest(string method, Uri url, Account account, IDictionary<string, string> parameters = null)
            : base(method, url, parameters, account)
        {
        }

        protected new HttpWebRequest GetPreparedWebRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(GetPreparedUrl());
            request.Method = Method;
            request.CookieContainer = Account.Cookies;
            request.Headers[AuthtenticationHeaderKey] = GetAuthentcationHeader(Account);
            return request;
        }

        public static string GetAuthentcationHeader(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            if (!account.Properties.ContainsKey("access_token"))
            {
                throw new ArgumentException("OAuth2 account is missing required access_token property.", "account");
            }

            return account.Properties["access_token"];
        }

    }
}
