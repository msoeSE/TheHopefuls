using System;
using System.Linq;
using OAuth.StudentDriver;
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

		protected override async void OnStart ()
		{
		    if (!await WebService.Instance.UserLoggedIn()) LoginAction();
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
