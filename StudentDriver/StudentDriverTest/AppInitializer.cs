using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace StudentDriverTest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                string currendDir = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                var apkLocation = Directory.GetParent(currendDir).Parent.Parent.Parent.GetFiles("Droid\\bin\\Release\\io.patz.driving_log.apk").FirstOrDefault().FullName;

                return ConfigureApp
                    .Android
                    .ApkFile(apkLocation)
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                // TODO: Update this path to point to your iOS app and uncomment the
                // code if the app is not included in the solution.
                //.AppBundle ("../../../iOS/bin/iPhoneSimulator/Debug/XamarinForms.iOS.app")
                .StartApp();
        }
    }
}

