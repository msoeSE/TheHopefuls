using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using Xamarin.Forms;
using StudentDriver.Droid;
using Android.App;
using System.Net.Http;
using StudentDriver.Helpers;
using Xamarin.Auth;
using StudentDriver.Services;
using Acr.UserDialogs;


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
			var auth = new OAuth2Authenticator (
				clientId: OAuth.FACEBOOK_APP_ID,
				scope: "email",
				authorizeUrl: new Uri (OAuth.FACEBOOK_OAUTH_URL),
				redirectUrl: new Uri (OAuth.FACEBOOK_SUCCESS));
			auth.AllowCancel = true;
			auth.Title = "Connect to Facebook";
			auth.Completed += async (sender, ev) => {
				if (!ev.IsAuthenticated) {
					App.Current.MainPage = new LoginPage ();
					return;
				} else {
					UserDialogs.Instance.Loading ("Logging In...");
					var access = ev.Account.Properties ["access_token"];
					using (var client = new HttpClient ()) {
						if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Google, access)) {
							WebService.GetInstance ().SetTokenHeader (access);
							Settings.AccessToken = access;
							App.Current.MainPage = new StudentDriverPage ();
						} else {
							App.Current.MainPage = new LoginPage ();
						}
					}
				}
				UserDialogs.Instance.HideLoading ();
			};
			this.Context.StartActivity (auth.GetUI (this.Context));
		}
	}

}

