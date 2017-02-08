using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using StudentDriver.iOS;
using StudentDriver;
using Xamarin.Forms;
using Xamarin.Auth;
using StudentDriver.Helpers;
using StudentDriver.Services;
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;

[assembly: ExportRenderer (typeof (FacebookLoginPage), typeof (FacebookLoginPageRenderer))]

namespace StudentDriver.iOS
{
	public class FacebookLoginPageRenderer : PageRenderer
	{
		public FacebookLoginPageRenderer ()
		{

		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			var auth = new OAuth2Authenticator (
											   clientId: OAuth.FACEBOOK_APP_ID,
											   scope: "",
											   authorizeUrl: new Uri (OAuth.FACEBOOK_OAUTH_URL),
											   redirectUrl: new Uri (OAuth.FACEBOOK_SUCCESS));
			auth.AllowCancel = false;
			auth.Title = "Connect to Facebook";
			auth.Completed += async (sender, e) => {
				if (!e.IsAuthenticated) {
					DismissViewController (true, App.LoginAction);
					UserDialogs.Instance.Alert ("Unable to Login, user not authenticated. Please Try Again", "Error", "Okay");
					return;
				} else {
					UserDialogs.Instance.Loading ("Logging In...");
					var access = e.Account.Properties ["access_token"];
					if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Facebook, access)) {
						Settings.OAuthAccessToken = access;
						Settings.OAuthSourceProvider = WebService.OAuthSource.Facebook;
						SaveFacebookProfile(e.Account);
						DismissViewController (true, App.SuccessfulLoginAction);

					} else {
						DismissViewController (true, App.LoginAction);
						UserDialogs.Instance.Alert ("Unable to Login, Please Try Again", "Error", "Okay");
					}
					UserDialogs.Instance.HideLoading ();

				}
			};

			UIViewController vc = auth.GetUI ();
			ViewController.AddChildViewController (vc);
			ViewController.View.Add (vc.View);
			vc.ChildViewControllers [0].NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, async (o, e) => await App.Current.MainPage.Navigation.PopModalAsync ());

		}

        private async void SaveFacebookProfile(Account account)
        {
            var request = new OAuth2Request("GET", new Uri(OAuth.FACEBOOK_PROFILE_REQUEST_URL), null, account);

            await request.GetResponseAsync().ContinueWith(async t =>
            {
                if (t.IsFaulted)
                {
                    return;
                }
                var json = JObject.Parse(t.Result.GetResponseText());
                var user = SQLiteDatabase.GetInstance().GetUser().Result;
                user.FirstName = json["name"].ToString();
                user.ImageUrl = json["picture"]["data"]["url"].ToString();
                await SQLiteDatabase.GetInstance().UpdateUser(user);
                WebService.GetInstance().SetTokenHeader();
            });
        }


    }
}
