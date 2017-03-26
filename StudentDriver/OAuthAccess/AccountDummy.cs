using System.Collections.Generic;
using System.Net;

namespace OAuthAccess
{
    public class AccountDummy
    {
        public string Username { get; set; }
        public Dictionary<string, string> Properties { get; private set; }
        public CookieContainer Cookies { get; private set; }

        public AccountDummy(string username, Dictionary<string,string> properties, CookieContainer cookies )
        {
            Username = username;
            Properties = properties;
            Cookies = cookies;
        }
    }
}



