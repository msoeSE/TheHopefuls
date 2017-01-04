using System;
using System.Xml;
using System.Diagnostics;
using System.Xml.Linq;
using System.Reflection;
namespace StudentDriver
{
	public class OAuth
	{
		public static string FACEBOOK_APP_ID { get; private set; }
		public static string FACEBOOK_SECRET_ID { get; private set; }
		public static string FACEBOOK_OAUTH_URL { get; private set; }
		public static string FACEBOOK_SUCCESS { get; private set; }
		public static string GOOGLE_APP_ID { get; private set; }
		public static string GOOGLE_SECRET_ID { get; private set; }
		public static string GOOGLE_OAUTH_URL { get; private set; }
		public static string GOOGLE_SUCCESS { get; private set; }

		public OAuth ()
		{

		}

		public static void InitializeKeys ()
		{
			var assembly = typeof (OAuth).GetTypeInfo ().Assembly;
			var fileStream = assembly.GetManifestResourceStream ("StudentDriver.Keys.xml");
			using (XmlReader reader = XmlReader.Create (fileStream)) {
				reader.MoveToContent ();
				while (reader.Read ()) {
					if (reader.IsStartElement ()) {
						switch (reader.Name) {
						case "Google":
							GOOGLE_APP_ID = reader ["id"];
							GOOGLE_SECRET_ID = reader ["secret"];
							GOOGLE_OAUTH_URL = reader ["oauthURL"];
							GOOGLE_SUCCESS = reader ["redirectURL"];
							break;

						case "Facebook":
							FACEBOOK_APP_ID = reader ["id"];
							FACEBOOK_SECRET_ID = reader ["secret"];
							FACEBOOK_OAUTH_URL = reader ["oauthURL"];
							FACEBOOK_SUCCESS = reader ["redirectURL"];
							break;
						}
					}
				}
			}
		}
	}
}
