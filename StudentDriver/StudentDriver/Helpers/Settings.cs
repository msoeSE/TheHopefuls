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

        public static string APIBaseUrl
        {
            get
            {
#if DEBUG
                return apiDevBaseUrl;
#else
                return apiProdBaseUrl
#endif
            }
        }

        public static string APIBasePort
        {
            get
            {
#if DEBUG
                return apiDevBasePort;
#else
                return apiProdBasePort
#endif
            }
        }

        public const string OAuthEndpoint = "auth/facebook/token";

    }
}