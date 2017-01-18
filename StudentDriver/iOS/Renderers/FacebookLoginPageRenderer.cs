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
				DismissViewController (true, null);
				if (!e.IsAuthenticated) {
					return;
				}
			    var access = e.Account.Properties ["access_token"];
			    if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Facebook, access)) {
			        Settings.OAuthAccessToken = access;
			        Settings.OAuthSourceProvier = WebService.OAuthSource.Facebook;
			        WebService.GetInstance ().SetTokenHeader ();
			        App.SucessfulLoginAction();
			    }
			};

			UIViewController vc = auth.GetUI ();
			ViewController.AddChildViewController (vc);
			ViewController.View.Add (vc.View);
			vc.ChildViewControllers [0].NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, async (o, e) => await App.Current.MainPage.Navigation.PopModalAsync ());
		}


	}
}
