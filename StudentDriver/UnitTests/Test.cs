using NUnit.Framework;
using System;
using Moq;
using StudentDriver;
using StudentDriver.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace UnitTests
{
	[TestFixture ()]
	public class Test
	{
		[TestFixtureSetUp ()]
		public void Setup ()
		{
            DependencyService.Register<ISQLite>();
		    SQLiteDatabase.GetInstance().DeleteDatabase();
		}

        [TearDown()]
        public void TearDown()
        {
            SQLiteDatabase.GetInstance().DeleteDatabase();
        }

        [Test ()]
		public async void TestAddUser_UserCreated_StoredInDatabase()
        {
            var testUserName = "TestUser";
            await SQLiteDatabase.GetInstance().AddUser(new User {FirstName = testUserName });
            var userAdded = SQLiteDatabase.GetInstance().GetUser();
            Assert.AreEqual(testUserName,userAdded.Result.FirstName);
        }
	}
}
