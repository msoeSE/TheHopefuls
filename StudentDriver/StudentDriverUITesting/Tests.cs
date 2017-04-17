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
            app.Tap(x => x.Class("FrameRenderer"));
            app.Tap(x => x.Css("input"));
            app.EnterText("email");
            app.EnterText("\t");
            app.EnterText("password");
            app.Tap(x => x.Css("button"));
        }
	}
}
