using StudentDriver.Autofac;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using UIKit;

namespace StudentDriver.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			ImageCircleRenderer.Init();
			LoadApplication (new App (new AppSetup()));

			return base.FinishedLaunching (app, options);
		}
	}
}
