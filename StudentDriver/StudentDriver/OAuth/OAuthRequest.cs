using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace OAuth
{
    public class OAuthRequest : Request
    {
        private const string AccessTokenKey = "access_token";

        public OAuthRequest(string method, Uri url, IDictionary<string, string> parameters = null, Account account = null)
            : base(method, url, parameters, account)
        {
        }
        public override Task<Response> GetResponseAsync(CancellationToken cancellationToken)
        {
            if (Account == null)
            {
                throw new InvalidOperationException("You must specify an Account for this request to proceed");
            }

            var req = GetPreparedWebRequest();

            var tmp = GetAuthorizationHeader(Account);
            req.Headers.Add(AccessTokenKey, tmp);
            return base.GetResponseAsync(cancellationToken);
        }
        public static string GetAuthorizationHeader(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (!account.Properties.ContainsKey("access_token"))
            {
                throw new ArgumentException("OAuth2 account is missing required access_token property.", "account");
            }

            return account.Properties["access_token"];
        }
    }
}
