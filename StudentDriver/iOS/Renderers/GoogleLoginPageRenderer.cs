﻿using System;
using Xamarin.Forms.Platform.iOS;
using StudentDriver;
using Xamarin.Forms;
using StudentDriver.iOS;
using Xamarin.Auth;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using StudentDriver.Helpers;
using UIKit;
using StudentDriver.Services;
using StudentDriver.Helpers;
using Acr.UserDialogs;

[assembly: ExportRenderer (typeof (GoogleLoginPage), typeof (GoogleLoginPageRenderer))]
namespace StudentDriver.iOS
{
	public class GoogleLoginPageRenderer : PageRenderer
	{
		public GoogleLoginPageRenderer ()
		{
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			var auth = new OAuth2Authenticator (
				clientId: OAuth.GOOGLE_APP_ID,
				scope: "email",
				authorizeUrl: new Uri (OAuth.GOOGLE_OAUTH_URL),
				redirectUrl: new Uri (OAuth.GOOGLE_SUCCESS));
			auth.AllowCancel = false;
			auth.Title = "Connect to Google";
			auth.Completed += async (sender, e) => {
				if (!e.IsAuthenticated) {
					DismissViewController (true, () => {
						App.Current.MainPage = new LoginPage ();
					});
					return;
				} else {
					UserDialogs.Instance.Loading ("Logging In...");
					var access = e.Account.Properties ["access_token"];
					using (var client = new HttpClient ()) {
						if (await WebService.GetInstance ().PostOAuthToken (WebService.OAuthSource.Google, access)) {
							Settings.OAuthAccessToken = access;
							Settings.OAuthSourceProvier = WebService.OAuthSource.Google;
							WebService.GetInstance().SetTokenHeader();
							App.SucessfulLoginAction();
							DismissViewController (true, () => {
								App.Current.MainPage = new StudentDriverPage ();
							});
						} else {
							DismissViewController (true, () => {
								App.Current.MainPage = new LoginPage ();
							});
						}
					}
				}
				UserDialogs.Instance.HideLoading ();
			};

			UIViewController vc = auth.GetUI ();
			ViewController.AddChildViewController (vc);
			ViewController.View.Add (vc.View);
			vc.ChildViewControllers [0].NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, async (o, e) => await App.Current.MainPage.Navigation.PopModalAsync ());
		}
	}
}
