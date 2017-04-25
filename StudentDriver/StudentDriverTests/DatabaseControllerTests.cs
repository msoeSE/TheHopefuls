using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OAuth;
using StudentDriver;
using StudentDriver.Helpers;
using StudentDriver.Models;
using StudentDriver.Services;

namespace StudentDriverTests
{
    [TestClass]
    public class DatabaseControllerTests
    {
        [TestInitialize]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        [TestMethod]
        public async Task SaveUser_EmptyString_ReturnFalse()
        {
            var mockSqLiteDatabase = new Mock<ISQLiteDatabase>();
            var databaseController = new DatabaseController(mockSqLiteDatabase.Object);
            var userSaved = await databaseController.SaveUser(string.Empty);
            Assert.IsFalse(userSaved);
        }

        [TestMethod]
        public async Task SaveUser_MissingProperty_ReturnFalse()
        {
            var userProfileJson = "{\"provider\":\"facebook\",\"displayName\":\"Hardip Gill\",\"name\":{\"familyName\":\"Gill\",\"givenName\":\"Hardip\",\"middleName\":\"\"},\"gender\":\"\",\"emails\":[{\"value\":\"\"}],\"photos\":[{\"value\":\"https://graph.facebook.com/v2.6/1148274255285331/picture?type=large\"}],\"_raw\":{\"id\":\"1148274255285331\",\"name\":\"Hardip Gill\",\"last_name\":\"Gill\",\"first_name\":\"Hardip\"},\"_json\":{\"id\":\"1148274255285331\",\"name\":\"Hardip Gill\",\"last_name\":\"Gill\",\"first_name\":\"Hardip\"},\"userType\":\"student\"}\r\n";
            var mockSqLiteDatabase = new Mock<ISQLiteDatabase>();
            var databaseController = new DatabaseController(mockSqLiteDatabase.Object);
            var userSaved = await databaseController.SaveUser(userProfileJson);
            Assert.IsFalse(userSaved);
        }

        [TestMethod]
        public async Task SaveUser_ValidUserProfileJson_UserUpdatedCorrectly()
        {
            var userProfileJson = "{\"provider\":\"facebook\",\"id\":\"1148274255285331\",\"displayName\":\"Hardip Gill\",\"name\":{\"familyName\":\"Gill\",\"givenName\":\"Hardip\",\"middleName\":\"\"},\"gender\":\"\",\"emails\":[{\"value\":\"\"}],\"photos\":[{\"value\":\"https://graph.facebook.com/v2.6/1148274255285331/picture?type=large\"}],\"_raw\":{\"id\":\"1148274255285331\",\"name\":\"Hardip Gill\",\"last_name\":\"Gill\",\"first_name\":\"Hardip\"},\"_json\":{\"id\":\"1148274255285331\",\"name\":\"Hardip Gill\",\"last_name\":\"Gill\",\"first_name\":\"Hardip\"},\"userType\":\"student\",\"mongoID\":\"58ee4e2178059559cffc9614\"}\r\n";
            var mockSqLiteDatabase = new Mock<ISQLiteDatabase>();
            var user = new User();
            mockSqLiteDatabase.Setup(x => x.GetUser()).ReturnsAsync(user);
            var databaseController = new DatabaseController(mockSqLiteDatabase.Object);
            await databaseController.SaveUser(userProfileJson);

            var expectedUser = new User
            {
                FirstName = "Hardip",
                LastName = "Gill",
                ImageUrl = "https://graph.facebook.com/v2.6/1148274255285331/picture?type=large",
                UType = User.UserType.Student,
                ServerId = "58ee4e2178059559cffc9614"
            };
            Assert.AreEqual(expectedUser.FirstName,user.FirstName);
            Assert.AreEqual(expectedUser.LastName, user.LastName);
            Assert.AreEqual(expectedUser.ImageUrl, user.ImageUrl);
            Assert.AreEqual(expectedUser.UType, user.UType);
            Assert.AreEqual(expectedUser.ServerId, user.ServerId);
        }

        [TestMethod]
        public async Task ConnectStudentToDrivingSchool_UserJsonEmpty_ReturnFalse()
        {
            var mockSqLiteDatabase = new Mock<ISQLiteDatabase>();
            var databaseController = new DatabaseController(mockSqLiteDatabase.Object);
            var userConnectedToDrivingSchool = await databaseController.ConnectStudentToDrivingSchool(string.Empty);
            Assert.IsFalse(userConnectedToDrivingSchool);
        }

        [TestMethod]
        public async Task ConnectStudentToDrivingSchool_UserJsonMissingProperty_ReturnFalse()
        {
            var userProfileJson = "{\"_id\":\"58ee4e2178059559cffc9614\",\"userId\":\"1148274255285331\",\"service\":\"facebook\",\"firstName\":\"Hardip\",\"lastName\":\"Gill\",\"userType\":\"student\",\"__v\":0,\"drivingSessions\":[]}\r\n";
            var mockSqLiteDatabase = new Mock<ISQLiteDatabase>();
            var databaseController = new DatabaseController(mockSqLiteDatabase.Object);
            var userConnectedToDrivingSchool = await databaseController.ConnectStudentToDrivingSchool(userProfileJson);
            Assert.IsFalse(userConnectedToDrivingSchool);
        }

        [TestMethod]
        public async Task ConnectStudentToDrivingSchool_UserJsonCorrect_UpdateUserCorrectly()
        {
            var userProfileJson = "{\"_id\":\"58ee4e2178059559cffc9614\",\"userId\":\"1148274255285331\",\"service\":\"facebook\",\"firstName\":\"Hardip\",\"lastName\":\"Gill\",\"userType\":\"student\",\"__v\":0,\"schoolId\":123,\"drivingSessions\":[]}\r\n";
            var mockSqLiteDatabase = new Mock<ISQLiteDatabase>();
            var user = new User();
            mockSqLiteDatabase.Setup(x => x.GetUser()).ReturnsAsync(user);
            var databaseController = new DatabaseController(mockSqLiteDatabase.Object);
            await databaseController.ConnectStudentToDrivingSchool(userProfileJson);
            var expectedSchoolId = "123";
            Assert.AreEqual(expectedSchoolId,user.DrivingSchoolId);
        }
    }
}
