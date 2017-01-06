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
			auth.AllowCancel = false;
			auth.Completed += async (sender, ev) => {
				if (!ev.IsAuthenticated) {
					return;
				} else {
					var access = ev.Account.Properties ["access_token"];
					using (var client = new HttpClient ()) {
						var content = new FormUrlEncodedContent (new [] {
							new KeyValuePair<string,string>("accessToken",access),
						});
						var authResponse = await client.PostAsync (new Uri ("https://host.dylanwalseth.me/auth/facebook/token"), content);
						if (authResponse.IsSuccessStatusCode) {
							var responseContent = await authResponse.Content.ReadAsStringAsync ();
							var authTicket = JsonConvert.DeserializeObject<AuthenticatedUser> (responseContent);
							//TODO Find out what dylan is sending
							if (authTicket != null) {
								var apiAccessToken = authTicket.Access_Token;
							}
						}
					}
				}
			};
			activity.StartActivity (auth.GetUI (activity));
		}

		public class AuthenticatedUser
		{
			public string Access_Token {
				get;
				set;
			}
		}
	}
}

