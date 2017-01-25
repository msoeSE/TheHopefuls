using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using StudentDriver.Droid;
using Xamarin.Forms;
using Xamarin.Auth;
using StudentDriver.Helpers;
using StudentDriver.Services;
using Acr.UserDialogs;

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
					App.LoginAction.Invoke ();
					UserDialogs.Instance.Alert ("Unable to Login, user not authenticated. Please Try Again", "Error", "Okay");
					return;
				} else {
					UserDialogs.Instance.Loading ("Logging In...");
					var access = ev.Account.Properties ["access_token"];
					if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Google, access)) {
						Settings.OAuthAccessToken = access;
						Settings.OAuthSourceProvider = WebService.OAuthSource.Google;
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
	}
}
