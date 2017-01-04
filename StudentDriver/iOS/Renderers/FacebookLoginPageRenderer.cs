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
			auth.Completed += async (sender, e) => {
				DismissViewController (true, null);
				if (!e.IsAuthenticated) {
					return;
				} else {
					var access = e.Account.Properties ["access_token"];
					using (var client = new HttpClient ()) {
						var content = new FormUrlEncodedContent (new [] {
							new KeyValuePair<string,string>("accessToken", access),
						});
						var authResponse = await client.PostAsync (new Uri ("https://host.walseth.me/auth/facebook/token"), content);
						if (authResponse.IsSuccessStatusCode) {
							var responseContent = await authResponse.Content.ReadAsStringAsync ();
							var authTicket = JsonConvert.DeserializeObject<AuthenticatedUser> (responseContent);
							//TODO Find out what Dylan is sending.
							if (authTicket != null) {
								var apiAccessToken = authTicket.Access_Token;

							}
						}
					}

				}
			};

			UIViewController vc = auth.GetUI ();
			ViewController.AddChildViewController (vc);
			ViewController.View.Add (vc.View);
			vc.ChildViewControllers [0].NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, async (o, e) => await App.Current.MainPage.Navigation.PopModalAsync ());
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
