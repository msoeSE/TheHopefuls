using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace OAuthAccess
{
    public interface IAccountHandler
    {
        void SaveFacebookAccount(AccountDummy account);
        AccountDummy GetSavedFacebookAccount();
        void DeAuthenticateAccount();
    }
}
