// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

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
		private const string AccessKey = "accessToken";
		private static readonly string AccessTokenDefault = string.Empty;
		private const string OAuthKey = "OAuthSource";
		private static readonly string OAuthSourceDefault = string.Empty;
		private const string FACEBOOK_OAUTH = "facebook";
		private const string GOOGLE_OAUTH = "google";
		#endregion


		public static string OAuthSource {
			get {
				return AppSettings.GetValueOrDefault<string> (OAuthKey, OAuthSourceDefault);
			}
			set {
				AppSettings.AddOrUpdateValue<string> (OAuthKey, value);
			}
		}

		public static string AccessToken {
			get {
				return AppSettings.GetValueOrDefault<string> (AccessKey, AccessTokenDefault);
			}
			set {
				AppSettings.AddOrUpdateValue<string> (AccessKey, value);
			}
		}
		public static string GeneralSettings {
			get {
				return AppSettings.GetValueOrDefault<string> (SettingsKey, SettingsDefault);
			}
			set {
				AppSettings.AddOrUpdateValue<string> (SettingsKey, value);
			}
		}

	}
}