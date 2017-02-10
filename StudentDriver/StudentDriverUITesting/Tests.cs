using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace StudentDriverUITesting
{
	[TestFixture (Platform.Android)]
	//[TestFixture (Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests (Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
		}

		[Test]
		public void LogInWithFacebook_CorrectCredentials_CanSeeNameAndProfilePic ()
		{
            app.Repl();
		    app.WaitForElement(c => c.Marked("Driving Log"));
            app.Tap(c=>c.Marked("FrameRenderer"));
			app.Screenshot ("First screen.");
		}
	}
}
