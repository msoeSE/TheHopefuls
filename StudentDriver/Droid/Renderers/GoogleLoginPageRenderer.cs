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
