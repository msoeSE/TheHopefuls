using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OAuth;
using StudentDriver;
using StudentDriver.Helpers;
using StudentDriver.Models;
using StudentDriver.Services;
using Xamarin.Auth;

namespace StudentDriverTests
{
    [TestClass]
    public class ServiceControllerTests
    {
        [TestInitialize]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        [TestMethod]
        public async Task UserLoggedIn_EmptyUrl_ReturnFalse()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            mockOAuthController.Setup(x => x.VerifySavedAccount(string.Empty)).ReturnsAsync("");
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.UserLoggedIn();
            Assert.IsFalse(userLoggedIn);
        }

        [TestMethod]
        public async Task UserLoggedIn_InvalidUrl_ReturnFalse()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            mockOAuthController.Setup(x => x.VerifySavedAccount("dylan.suckballs.online")).ReturnsAsync("");
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.UserLoggedIn();
            Assert.IsFalse(userLoggedIn);
        }

        [TestMethod]
        public async Task UserLoggedIn_NullAccount_ReturnFalse()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            mockOAuthController.Setup(x => x.VerifySavedAccount(Settings.OAuthUrl)).ReturnsAsync("");
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.UserLoggedIn();
            Assert.IsFalse(userLoggedIn);
        }

        [TestMethod]
        public async Task UserLoggedIn_EmptyResponse_ReturnFalse()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            mockOAuthController.Setup(x => x.VerifySavedAccount(Settings.OAuthUrl)).ReturnsAsync("");
            var serviceController = new ServiceController(mockOAuthController.Object,mockDatabaseController.Object);
            var userLoggedIn = await serviceController.UserLoggedIn();
            Assert.IsFalse(userLoggedIn);
        }

        [TestMethod]
        public async Task UserLoggedIn_SuccesfulReponseNotInCorrectFormat_ReturnFalse()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var responseText = "Dylan is an idiot";
            mockOAuthController.Setup(x => x.VerifySavedAccount(Settings.OAuthUrl)).ReturnsAsync(responseText);
            mockDatabaseController.Setup(x => x.SaveUser(responseText)).ReturnsAsync(false);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.UserLoggedIn();
            Assert.IsFalse(userLoggedIn);
        }

        [TestMethod]
        public async Task UserLoggedIn_SuccesfulReponseInCorrectFormat_ReturnTrue()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var responseText = "correctFormat";
            mockOAuthController.Setup(x => x.VerifySavedAccount(Settings.OAuthUrl)).ReturnsAsync(responseText);
            mockDatabaseController.Setup(x => x.SaveUser(responseText)).ReturnsAsync(true);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.UserLoggedIn();
            Assert.IsTrue(userLoggedIn);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_StateStringEmpty_ReturnNull()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.GetAggregatedDrivingData(string.Empty, "");
            Assert.IsNull(userLoggedIn);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_ResponseEmpty_ReturnNull()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(string.Empty);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var userLoggedIn = await serviceController.GetAggregatedDrivingData("Wisconsin", "");
            Assert.IsNull(userLoggedIn);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_SuccessfulResponse_ReturnCorrctDrivingDataViewModel()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var response = "{\"dayHours\":1,\"nightHours\":1,\"inclementHours\":1,\"totalHours\":2}\r\n";
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(response);
            var state = "Wisconsin";
            var stateReqs = new StateReqs
                            {
                                DayHours = 10,
                                NightHours = 10,
                                InclementWeatherHours = 10,
                                State = state
            };
            mockDatabaseController.Setup(x => x.GetStateRequirements(state)).ReturnsAsync(stateReqs);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var drivingDataViewModel = await serviceController.GetAggregatedDrivingData("Wisconsin", "");

            
            Assert.AreEqual(0.1,drivingDataViewModel.Total.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.Total.PercentCompletedString);
            Assert.AreEqual("2.0/20", drivingDataViewModel.Total.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalDayTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalDayTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalDayTime.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalNightTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalNightTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalNightTime.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalInclement.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalInclement.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalInclement.RatioString);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_SuccessfulResponse_InclementWeather_ReturnCorrctDrivingDataViewModel()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var response = "{\"dayHours\":1,\"nightHours\":1,\"inclementHours\":1,\"totalHours\":2}\r\n";
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(response);
            var state = "Wisconsin";
            var stateReqs = new StateReqs
            {
                DayHours = 10,
                NightHours = 10,
                NightOrInclement = true,
                State = state
            };
            mockDatabaseController.Setup(x => x.GetStateRequirements(state)).ReturnsAsync(stateReqs);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var drivingDataViewModel = await serviceController.GetAggregatedDrivingData("Wisconsin", "");


            Assert.AreEqual(0.15, drivingDataViewModel.Total.PercentCompletedDouble);
            Assert.AreEqual("15.00 %", drivingDataViewModel.Total.PercentCompletedString);
            Assert.AreEqual("3.0/20", drivingDataViewModel.Total.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalDayTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalDayTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalDayTime.RatioString);

            Assert.AreEqual(0.2, drivingDataViewModel.TotalNightTime.PercentCompletedDouble);
            Assert.AreEqual("20.00 %", drivingDataViewModel.TotalNightTime.PercentCompletedString);
            Assert.AreEqual("2.0/10", drivingDataViewModel.TotalNightTime.RatioString);

            Assert.AreEqual(0.0, drivingDataViewModel.TotalInclement.PercentCompletedDouble);
            Assert.AreEqual("0.00 %", drivingDataViewModel.TotalInclement.PercentCompletedString);
            Assert.AreEqual("0.0/0", drivingDataViewModel.TotalInclement.RatioString);
        }


        [TestMethod]
        public async Task GetAggregatedDrivingData_SuccessfulResponse_CompletedRequirementNotIncludingInclement_ReturnCorrctDrivingDataViewModel()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var response = "{\"dayHours\":10,\"nightHours\":1,\"inclementHours\":1,\"totalHours\":2}\r\n";
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(response);
            var state = "Wisconsin";
            var stateReqs = new StateReqs
            {
                DayHours = 10,
                NightHours = 10,
                NightOrInclement = false,
                State = state
            };
            mockDatabaseController.Setup(x => x.GetStateRequirements(state)).ReturnsAsync(stateReqs);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var drivingDataViewModel = await serviceController.GetAggregatedDrivingData("Wisconsin", "");


            Assert.AreEqual(0.55, drivingDataViewModel.Total.PercentCompletedDouble);
            Assert.AreEqual("55.00 %", drivingDataViewModel.Total.PercentCompletedString);
            Assert.AreEqual("11.0/20", drivingDataViewModel.Total.RatioString);

            Assert.AreEqual(1.0, drivingDataViewModel.TotalDayTime.PercentCompletedDouble);
            Assert.AreEqual("100.00 %", drivingDataViewModel.TotalDayTime.PercentCompletedString);
            Assert.AreEqual("10.0/10", drivingDataViewModel.TotalDayTime.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalNightTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalNightTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalNightTime.RatioString);

            Assert.AreEqual(0.0, drivingDataViewModel.TotalInclement.PercentCompletedDouble);
            Assert.AreEqual("0.00 %", drivingDataViewModel.TotalInclement.PercentCompletedString);
            Assert.AreEqual("0.0/0", drivingDataViewModel.TotalInclement.RatioString);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_SuccessfulResponse_CompletedRequirementIncludingInclement_ReturnCorrctDrivingDataViewModel()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var response = "{\"dayHours\":1,\"nightHours\":5,\"inclementHours\":5,\"totalHours\":2}\r\n";
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(response);
            var state = "Wisconsin";
            var stateReqs = new StateReqs
            {
                DayHours = 10,
                NightHours = 10,
                NightOrInclement = true,
                State = state
            };
            mockDatabaseController.Setup(x => x.GetStateRequirements(state)).ReturnsAsync(stateReqs);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var drivingDataViewModel = await serviceController.GetAggregatedDrivingData("Wisconsin", "");


            Assert.AreEqual(0.55, drivingDataViewModel.Total.PercentCompletedDouble);
            Assert.AreEqual("55.00 %", drivingDataViewModel.Total.PercentCompletedString);
            Assert.AreEqual("11.0/20", drivingDataViewModel.Total.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalDayTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalDayTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalDayTime.RatioString);

            Assert.AreEqual(1.0, drivingDataViewModel.TotalNightTime.PercentCompletedDouble);
            Assert.AreEqual("100.00 %", drivingDataViewModel.TotalNightTime.PercentCompletedString);
            Assert.AreEqual("10.0/10", drivingDataViewModel.TotalNightTime.RatioString);

            Assert.AreEqual(0.0, drivingDataViewModel.TotalInclement.PercentCompletedDouble);
            Assert.AreEqual("0.00 %", drivingDataViewModel.TotalInclement.PercentCompletedString);
            Assert.AreEqual("0.0/0", drivingDataViewModel.TotalInclement.RatioString);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_SuccessfulResponse_ExceededRequirementNotIncludingInclement_ReturnCorrctDrivingDataViewModel()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var response = "{\"dayHours\":1,\"nightHours\":11,\"inclementHours\":5,\"totalHours\":2}\r\n";
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(response);
            var state = "Wisconsin";
            var stateReqs = new StateReqs
            {
                DayHours = 10,
                NightHours = 10,
                NightOrInclement = false,
                State = state
            };
            mockDatabaseController.Setup(x => x.GetStateRequirements(state)).ReturnsAsync(stateReqs);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var drivingDataViewModel = await serviceController.GetAggregatedDrivingData("Wisconsin", "");


            Assert.AreEqual(0.55, drivingDataViewModel.Total.PercentCompletedDouble);
            Assert.AreEqual("55.00 %", drivingDataViewModel.Total.PercentCompletedString);
            Assert.AreEqual("11.0/20", drivingDataViewModel.Total.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalDayTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalDayTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalDayTime.RatioString);

            Assert.AreEqual(1.0, drivingDataViewModel.TotalNightTime.PercentCompletedDouble);
            Assert.AreEqual("100.00 %", drivingDataViewModel.TotalNightTime.PercentCompletedString);
            Assert.AreEqual("10.0/10", drivingDataViewModel.TotalNightTime.RatioString);

            Assert.AreEqual(0.0, drivingDataViewModel.TotalInclement.PercentCompletedDouble);
            Assert.AreEqual("0.00 %", drivingDataViewModel.TotalInclement.PercentCompletedString);
            Assert.AreEqual("0.0/0", drivingDataViewModel.TotalInclement.RatioString);
        }

        [TestMethod]
        public async Task GetAggregatedDrivingData_SuccessfulResponse_ExceededRequirementIncludingInclement_ReturnCorrctDrivingDataViewModel()
        {
            var mockDatabaseController = new Mock<IDatabaseController>();
            var mockOAuthController = new Mock<IOAuthController>();
            var response = "{\"dayHours\":1,\"nightHours\":5,\"inclementHours\":6,\"totalHours\":2}\r\n";
            mockOAuthController.Setup(x => x.MakeGetRequest(Settings.AggregateDrivingUrl, null)).ReturnsAsync(response);
            var state = "Wisconsin";
            var stateReqs = new StateReqs
            {
                DayHours = 10,
                NightHours = 10,
                NightOrInclement = true,
                State = state
            };
            mockDatabaseController.Setup(x => x.GetStateRequirements(state)).ReturnsAsync(stateReqs);
            var serviceController = new ServiceController(mockOAuthController.Object, mockDatabaseController.Object);
            var drivingDataViewModel = await serviceController.GetAggregatedDrivingData("Wisconsin", "");


            Assert.AreEqual(0.55, drivingDataViewModel.Total.PercentCompletedDouble);
            Assert.AreEqual("55.00 %", drivingDataViewModel.Total.PercentCompletedString);
            Assert.AreEqual("11.0/20", drivingDataViewModel.Total.RatioString);

            Assert.AreEqual(0.1, drivingDataViewModel.TotalDayTime.PercentCompletedDouble);
            Assert.AreEqual("10.00 %", drivingDataViewModel.TotalDayTime.PercentCompletedString);
            Assert.AreEqual("1.0/10", drivingDataViewModel.TotalDayTime.RatioString);

            Assert.AreEqual(1.0, drivingDataViewModel.TotalNightTime.PercentCompletedDouble);
            Assert.AreEqual("100.00 %", drivingDataViewModel.TotalNightTime.PercentCompletedString);
            Assert.AreEqual("10.0/10", drivingDataViewModel.TotalNightTime.RatioString);

            Assert.AreEqual(0.0, drivingDataViewModel.TotalInclement.PercentCompletedDouble);
            Assert.AreEqual("0.00 %", drivingDataViewModel.TotalInclement.PercentCompletedString);
            Assert.AreEqual("0.0/0", drivingDataViewModel.TotalInclement.RatioString);
        }


    }
}
