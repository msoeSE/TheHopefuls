using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using Xamarin.Forms;
using StudentDriver.Droid;
using Android.App;
using StudentDriver.Helpers;
using StudentDriver.Services;
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
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
		        clientId: OAuth.FACEBOOK_APP_ID,
		        scope: "email",
		        authorizeUrl: new Uri(OAuth.FACEBOOK_OAUTH_URL),
		        redirectUrl: new Uri(OAuth.FACEBOOK_SUCCESS))
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
					var access = ev.Account.Properties ["access_token"];
					if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Facebook, access)) {
                        //AccountStore.Create(Context).Save(ev.Account, "facebook");
                        Settings.OAuthAccessToken = access;
						Settings.OAuthSourceProvider = WebService.OAuthSource.Facebook;
                        SaveFacebookProfile(ev.Account);
                        WebService.GetInstance ().SetTokenHeader ();
						App.SuccessfulLoginAction.Invoke ();
					} else {
						App.LoginAction.Invoke ();
						UserDialogs.Instance.Alert ("Unable to Login, Please Try Again", "Error", "Okay");
					}
					UserDialogs.Instance.HideLoading ();
				}
			};
			this.Context.StartActivity (auth.GetUI (this.Context));
		}

        private void SaveFacebookProfile(Account account)
        {
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account);

            request.GetResponseAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    return;
                }
                var json = JObject.Parse(t.Result.GetResponseText());
                var user = SQLiteDatabase.GetInstance().GetUser().Result;
                user.FirstName = json["name"].ToString();
                SQLiteDatabase.GetInstance().UpdateUser(user);
            });
        }
    }

}

