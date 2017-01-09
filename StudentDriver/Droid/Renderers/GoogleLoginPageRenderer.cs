using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using StudentDriver.Droid;
using Xamarin.Forms;
using Xamarin.Auth;
using Android.App;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Widget;
using StudentDriver.Services;

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
					var access = ev.Account.Properties ["access_token"];
					using (var client = new HttpClient ()) {
						if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Google, access)) {
							WebService.GetInstance ().SetTokenHeader (access);
						}
					}
				}
			};
			this.Context.StartActivity (auth.GetUI (this.Context));
		}
	}

	public class AuthenticatedUser
	{
		public string Access_Token {
			get;
			set;
		}
	}
}
