using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using StudentDriver.Droid;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Net.Http;

using StudentDriver.Services;
using Acr.UserDialogs;
using StudentDriver.Helpers;

[assembly: ExportRenderer (typeof (GoogleLoginPage), typeof (GoogleLoginPageRenderer))]
namespace StudentDriver.Droid
{
	public class GoogleLoginPageRenderer : PageRenderer
	{
		public GoogleLoginPageRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);
			var auth = new OAuth2Authenticator (
				clientId: OAuth.GOOGLE_APP_ID,
				scope: "email",
				authorizeUrl: new Uri (OAuth.GOOGLE_OAUTH_URL),
				redirectUrl: new Uri (OAuth.GOOGLE_SUCCESS));
			auth.AllowCancel = true;
			auth.Title = "Connect to Google";
			auth.Completed += async (sender, ev) => {
				if (!ev.IsAuthenticated) {
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
			};
			this.Context.StartActivity (auth.GetUI (this.Context));
		}
	}
}
