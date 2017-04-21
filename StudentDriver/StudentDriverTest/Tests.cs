using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StudentDriver;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace StudentDriverTest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            var shit = new App(null);
        }
    }
}

