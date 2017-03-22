using System.Linq;
using OAuthAccess;
using Xamarin.Auth;

namespace StudentDriver.iOS
{
    public class AccountHandler_iOS: IAccountHandler
    {
        private const string ServiceId = "facebook";

        public AccountDummy GetSavedFacebookAccount()
        {
            var account = AccountStore.Create().FindAccountsForService(ServiceId).FirstOrDefault();
            return account == null ? null : new AccountDummy(account.Username, account.Properties, account.Cookies);
        }

        public void SaveFacebookAccount(AccountDummy dummyAccount)
        {
            var account = new Account(dummyAccount.Username, dummyAccount.Properties, dummyAccount.Cookies);
            AccountStore.Create().Save(account, ServiceId);
        }

        public void DeAuthenticateAccount()
        {
            AccountStore.Create().Delete(AccountStore.Create().FindAccountsForService(ServiceId).FirstOrDefault(), ServiceId);
        }
    }
}
