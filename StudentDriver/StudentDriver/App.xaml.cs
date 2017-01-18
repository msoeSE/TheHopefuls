using System;
using StudentDriver.Helpers;
using StudentDriver.Services;
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
			MainPage = new StudentDriverPage();
		}

		protected override void OnStart ()
		{
            OAuth.InitializeKeys();
            //Settings.OAuthAccessToken = "";
            //Settings.OAuthSourceProvier = WebService.OAuthSource.Facebook;
            var authenticated = WebService.GetInstance().PostOAuthToken(Settings.OAuthSourceProvier, Settings.OAuthAccessToken).Result;
            if (!authenticated)
            {
                LoginAction();
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

	    public static Action SucessfulLoginAction
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
