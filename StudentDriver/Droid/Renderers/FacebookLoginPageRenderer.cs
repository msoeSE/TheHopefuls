﻿using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using Xamarin.Forms;
using StudentDriver.Droid;
using Android.App;
using StudentDriver.Helpers;
using StudentDriver.Services;
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
using OAuth.StudentDriver;
using OAuthAccess;
using Xamarin.Auth;


[assembly: ExportRenderer (typeof (FacebookLoginPage), typeof (FacebookLoginPageRenderer))]

namespace StudentDriver.Droid
{
	public class FacebookLoginPageRenderer : PageRenderer
	{
		public FacebookLoginPageRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);
			var activity = this.Context as Activity;
		    var auth = new OAuth2Authenticator(
		        clientId: OAuthSettings.FACEBOOK_APP_ID,
		        scope: "",
		        authorizeUrl: new Uri(OAuthSettings.FACEBOOK_OAUTH_URL),
		        redirectUrl: new Uri(OAuthSettings.FACEBOOK_SUCCESS))
		    {
		        AllowCancel = true,
		        Title = "Connect to Facebook"
		    };
		    auth.Completed += async (sender, ev) => {
				if (!ev.IsAuthenticated) {
					App.LoginAction.Invoke ();
					UserDialogs.Instance.Alert ("Unable to Login, user not authenticated. Please Try Again", "Error", "Okay");
				} else {
					UserDialogs.Instance.Loading ("Logging In...");
                    var account = ev.Account;
                    if (!await WebService.Instance.SaveAccount(new AccountDummy(account.Username, account.Properties, account.Cookies)))
                    {
                        App.LoginAction.Invoke();
                        UserDialogs.Instance.Alert("Unable to Login, Please Try Again", "Error", "Okay");
                    }
                    App.SuccessfulLoginAction.Invoke();
                    UserDialogs.Instance.HideLoading();
                }
			};
			this.Context.StartActivity (auth.GetUI (this.Context));
		}

        //private async void SaveFacebookProfile(Account account)
        //{
        //    var request = new OAuth2Request("GET", new Uri(OAuth.FACEBOOK_PROFILE_REQUEST_URL), null, account);

        //    await request.GetResponseAsync().ContinueWith(async t =>
        //     {
        //         if (t.IsFaulted)
        //         {
        //             return;
        //         }
        //         var json = JObject.Parse(t.Result.GetResponseText());
        //         var user = SQLiteDatabase.GetInstance().GetUser().Result;
        //         user.FirstName = json["name"].ToString();
        //         user.ImageUrl = json["picture"]["data"]["url"].ToString();
        //         await SQLiteDatabase.GetInstance().UpdateUser(user);
        //         WebService.GetInstance().SetTokenHeader();
        //     });
        //}
    }

}

