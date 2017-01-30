using System;
using System.Linq;
using StudentDriver.Helpers;
using StudentDriver.Services;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace StudentDriver
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();
			MainPage = new StudentDriverPage ();
		}

		protected override void OnStart ()
		{
			OAuth.InitializeKeys ();
		    //var account = AccountStore.Create().FindAccountsForService("facebook").First();
		    Settings.OAuthAccessToken = "";
			var authenticated = WebService.GetInstance ().PostOAuthToken (Settings.OAuthSourceProvider, Settings.OAuthAccessToken).Result;
			if (!authenticated) {
				LoginAction ();
			}
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		public static Action SuccessfulLoginAction {
			get {
				return () => {
					Current.MainPage = new StudentDriverPage ();
				};
			}
		}

		public static Action LoginAction {
			get {
				return () => {
					Current.MainPage = new LoginPage ();
				};
			}
		}
	}
}
