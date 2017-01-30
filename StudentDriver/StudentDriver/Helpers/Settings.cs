// Helpers/Settings.cs
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
		private static ISettings AppSettings {
			get {
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;
		private static readonly int SettingsDefaultInt = 0;
		private const string FACEBOOK_OAUTH = "facebook";
		private const string GOOGLE_OAUTH = "google";
		#endregion


		private const string apiBaseUrl = "apiBaseUrl";
		private const string apiBasePort = "apiBasePort";
		private const string oAuthAccessToken = "oAuthAccessToken";
		private const string oAuthSourceProvider = "oAuthSourceProvider";


		public static string GeneralSettings {
			get {
				return AppSettings.GetValueOrDefault<string> (SettingsKey, SettingsDefault);
			}
			set {
				AppSettings.AddOrUpdateValue<string> (SettingsKey, value);
			}
		}

		public static string OAuthAccessToken {
			get { return AppSettings.GetValueOrDefault (oAuthAccessToken, SettingsDefault); }
			set { AppSettings.AddOrUpdateValue (oAuthAccessToken, value); }
		}

		public static WebService.OAuthSource OAuthSourceProvider {
			get { return AppSettings.GetValueOrDefault (oAuthSourceProvider, WebService.OAuthSource.None); }
			set { AppSettings.AddOrUpdateValue (oAuthSourceProvider, value); }
		}

		public static string APIBaseUrl {
			get { return AppSettings.GetValueOrDefault (apiBaseUrl, SettingsDefault); }
			set { AppSettings.AddOrUpdateValue (apiBaseUrl, value); }
		}

		public static int APIBasePort {
			get { return AppSettings.GetValueOrDefault (apiBasePort, SettingsDefaultInt); }
			set { AppSettings.AddOrUpdateValue (apiBasePort, value); }
		}

	}
}