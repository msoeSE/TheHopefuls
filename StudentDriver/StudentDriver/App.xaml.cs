﻿using System;
using System.Linq;
using StudentDriver.Helpers;
using StudentDriver.Services;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StudentDriver
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			MainPage = new StudentDriverPage();
		}

		protected override async void OnStart()
		{
			OAuth.InitializeKeys();
			//var account = AccountStore.Create().FindAccountsForService("facebook").First();
			var authenticated = WebService.GetInstance().PostOAuthToken(Settings.OAuthSourceProvider, Settings.OAuthAccessToken).Result;
#if DEBUG
			authenticated = true;
#endif
			if (!authenticated)
			{
				LoginAction();
			}
			else
			{
				//TODO check for database table of unsynced drive points, and then send it if it is there.
			}

			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		public static Action SuccessfulLoginAction
		{
			get
			{
				return () =>
				{
					Current.MainPage = new StudentDriverPage();
				};
			}
		}

		public static Action LoginAction
		{
			get
			{
				return () =>
				{
					Current.MainPage = new LoginPage();
				};
			}
		}
	}
}
