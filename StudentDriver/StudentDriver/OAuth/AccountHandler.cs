using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthAccess;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OAuth
{
	public static class AccountHandler
	{
		public static void SaveFacebookAccount(Account account)
		{
			var dummyAccount = new AccountDummy(account.Username, account.Properties, account.Cookies);
			DependencyService.Get<IAccountHandler>().SaveFacebookAccount(dummyAccount);
		}

		public static Account GetSavedFacebookAccount()
		{
			var dummyAccount = DependencyService.Get<IAccountHandler>().GetSavedFacebookAccount();
			if (dummyAccount == null) return null;
			var account = new Account(dummyAccount.Username, dummyAccount.Properties, dummyAccount.Cookies);
			return account;
		}

		public static void DeAuthenticateAccount()
		{
			DependencyService.Get<IAccountHandler>().DeAuthenticateAccount();
		}
	}
}
