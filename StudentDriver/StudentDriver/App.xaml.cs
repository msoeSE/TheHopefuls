using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using OAuth.StudentDriver;
using StudentDriver.Autofac;
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
		private static ServiceController _sc;
		public App(AppSetup setup)
		{
			InitializeComponent();
			var container = setup.CreateContainer();
			_sc = container.Resolve<IServiceController>() as ServiceController;
			MainPage = new StudentDriverPage();
		}

		protected override async void OnStart()
		{
            
            if (!await _sc.UserLoggedIn())
            {
                LoginAction();
            }
            //var userType = User.UserType.Instructor;
            Current.MainPage = new InstructorPage();
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

		public static IServiceController ServiceController => _sc;

		public async static Task<Action> SuccessfulLoginAction()
		{
			var userType = await ServiceController.GetUser();
			var page = Current.MainPage;
			if (userType.UType == User.UserType.Instructor)
			{
				page = new InstructorPage();
			}
			else
			{
				page = new StudentDriverPage();
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
