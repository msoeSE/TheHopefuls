using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace StudentDriverUITesting
{
	public class AppInitializer
	{
		public static IApp StartApp (Platform platform)
		{
			// TODO: If the iOS or Android app being tested is included in the solution 
			// then open the Unit Tests window, right click Test Apps, select Add App Project
			// and select the app projects that should be tested.
			//
			// The iOS project should have the Xamarin.TestCloud.Agent NuGet package
			// installed. To start the Test Cloud Agent the following code should be
			// added to the FinishedLaunching method of the AppDelegate:
			//
			//    #if ENABLE_TEST_CLOUD
			//    Xamarin.Calabash.Start();
			//    #endif
			if (platform == Platform.Android)
			{
                string currendDir = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
			    var apkLocation = Directory.GetParent(currendDir).Parent.Parent.Parent.GetFiles("Droid\\bin\\Release\\io.patz.driving_log.apk").FirstOrDefault().FullName;

                return ConfigureApp
					.Android
                    .ApkFile(apkLocation)
                    .StartApp ();
			}

			return ConfigureApp
				.iOS
				// TODO: Update this path to point to your iOS app and uncomment the
				// code if the app is not included in the solution.
				//.AppBundle ("../../../iOS/bin/iPhoneSimulator/Debug/XamarinForms.iOS.app")
				.StartApp ();
		}
	}
}
