using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDriver.Helpers
{
    public static class OAuthProvider
    {
        public static string ToString(this ProviderType providerType)
        {
            switch (providerType)
            {
                case ProviderType.Facebook:
                    return "facebook";
                case ProviderType.Google:
                    return "google";
            }
            return null;
        }

        public enum ProviderType
        {
            Facebook,
            Google
        }
    }
}
