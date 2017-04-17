// Helpers/Settings.cs

using System.Diagnostics;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StudentDriver.Services;

namespace StudentDriver.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{

        private const string apiDevBaseUrl = "dev.drivinglog.online/";
        private const string apiProdBaseUrl = "drivinglog.online/";
        private const string apiDevBasePort = "3000";
        private const string apiProdBasePort = "3000";

        public const string OAuthUrl = "https://dev.drivinglog.online/auth/facebook/token";
	    public const string SchoolIdUrl = "https://dev.drivinglog.online/api/linkacctoschool";
	    public const string WeatherUrl = "https://dev.drivinglog.online/api/weather";
        public const string AggregateDrivingUrl = "https://dev.drivinglog.online/api/totalDrivingData";
	    public const string StateReqUrl = "https://dev.drivinglog.online/api/statereq";
    }
}