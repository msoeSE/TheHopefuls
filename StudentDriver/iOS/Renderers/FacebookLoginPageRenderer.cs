using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using StudentDriver.iOS;
using StudentDriver;
using Xamarin.Forms;
using Xamarin.Auth;
using StudentDriver.Services;
using Acr.UserDialogs;
using OAuth.StudentDriver;
using OAuthAccess;

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
											   clientId: OAuthSettings.FACEBOOK_APP_ID,
											   scope: "",
											   authorizeUrl: new Uri (OAuthSettings.FACEBOOK_OAUTH_URL),
											   redirectUrl: new Uri (OAuthSettings.FACEBOOK_SUCCESS));
			auth.AllowCancel = false;
			auth.Title = "Connect to Facebook";
			auth.Completed += async (sender, e) => {
				if (!e.IsAuthenticated) {
					DismissViewController (true, App.LoginAction);
					UserDialogs.Instance.Alert ("Unable to Login, user not authenticated. Please Try Again", "Error", "Okay");
					return;
				} else {
					UserDialogs.Instance.Loading ("Logging In...");
				    var account = e.Account;
                    if (!await WebService.Instance.SaveAccount(new AccountDummy(account.Username,account.Properties,account.Cookies)))
                    {
                        App.LoginAction.Invoke();
                        UserDialogs.Instance.Alert("Unable to Login, Please Try Again", "Error", "Okay");
                    }
                    App.SuccessfulLoginAction.Invoke();
                    UserDialogs.Instance.HideLoading();

                }
			};

			UIViewController vc = auth.GetUI ();
			ViewController.AddChildViewController (vc);
			ViewController.View.Add (vc.View);
			vc.ChildViewControllers [0].NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, async (o, e) => await App.Current.MainPage.Navigation.PopModalAsync ());

		}


    }
}
