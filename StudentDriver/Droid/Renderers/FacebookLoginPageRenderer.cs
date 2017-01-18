using System;
using Xamarin.Forms.Platform.Android;
using StudentDriver;
using Xamarin.Forms;
using StudentDriver.Droid;
using Android.App;
using Xamarin.Auth;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using StudentDriver.Helpers;
using StudentDriver.Services;

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
					return;
				}
			    var access = ev.Account.Properties ["access_token"];
			    if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Facebook, access))
			    {
			        Settings.OAuthAccessToken = access;
			        Settings.OAuthSourceProvier = WebService.OAuthSource.Facebook;
			        WebService.GetInstance().SetTokenHeader();
			        App.SucessfulLoginAction();
			    }
			};
			activity.StartActivity (auth.GetUI (activity));
		}

	}
}

