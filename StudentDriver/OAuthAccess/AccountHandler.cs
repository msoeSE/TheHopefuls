using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace OAuth
{
    public static class AccountHandler
    {
        public static void SaveFacebookAccount(Account account)
        {
            AccountStore.Create().Save(account,"Facebook");
        }

        public static Account GetFacebookAccount()
        {
            return AccountStore.Create().FindAccountsForService("Facebook").FirstOrDefault();
        }
    }
}
