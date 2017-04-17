using System;
using System.Linq;
using OAuth.StudentDriver;
using StudentDriver.Helpers;
using StudentDriver.Models;
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
            if (!await ServiceController.Instance.UserLoggedIn())
            {
                LoginAction();
            }
            //var userType = User.UserType.Instructor;
		    var userType = await ServiceController.Instance.GetUserType();
            SuccessfulLoginAction(userType).Invoke();
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

	    public static Action SuccessfulLoginAction(User.UserType userType)
	    {
	        var page = Current.MainPage;
	        if (userType == User.UserType.Instructor)
	        {
	            page = new InstructorPage();
	        }

	        return () =>
	               {
	                   Current.MainPage = page;
	               };
	    }

	    public static Action StatsPageAction(string userid)
	    {
            var page = new StatsPage(userid);
            return () =>
            {
                Current.MainPage = page;
            };
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
