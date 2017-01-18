using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using StudentDriver.iOS;
using StudentDriver;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Net.Http;
using Splat;
using System.Collections.Generic;
using Newtonsoft.Json;
using StudentDriver.Helpers;
using StudentDriver.Services;
using StudentDriver.Helpers;
using Acr.UserDialogs;

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
											   scope: "email",
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
						WebService.GetInstance ().SetTokenHeader ();
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


	}
}
