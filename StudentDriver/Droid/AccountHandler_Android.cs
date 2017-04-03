using System.Linq;
using OAuthAccess;
using StudentDriver.Droid;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountHandler_Android))]

namespace StudentDriver.Droid
{
    public class AccountHandler_Android : IAccountHandler
    {
        private const string ServiceId = "facebook";

        public AccountDummy GetSavedFacebookAccount()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(ServiceId).FirstOrDefault();
            return account == null ? null : new AccountDummy(account.Username,account.Properties,account.Cookies);
        }

        public void SaveFacebookAccount(AccountDummy dummyAccount)
        {
            var account = new Account(dummyAccount.Username,dummyAccount.Properties,dummyAccount.Cookies);
            AccountStore.Create(Forms.Context).Save(account, ServiceId);
        }

        public void DeAuthenticateAccount()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(ServiceId).FirstOrDefault();
            if (account == null) return;
            AccountStore.Create(Forms.Context).Delete(account, ServiceId);
        }
    }

}