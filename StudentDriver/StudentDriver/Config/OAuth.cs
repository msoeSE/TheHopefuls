using System;
using System.Xml;
using System.Diagnostics;
using System.Xml.Linq;
using System.Reflection;
using System.Linq;
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

			XDocument doc = XDocument.Load (fileStream);
			var fbElement = doc.Element ("OAuth").Descendants ("Facebook");
			var googleElement = doc.Element ("OAuth").Descendants ("Google");
			FACEBOOK_OAUTH_URL = fbElement.Attributes ("oauthURL").FirstOrDefault ().Value;
			FACEBOOK_SUCCESS = fbElement.Attributes ("success").FirstOrDefault ().Value;
			GOOGLE_OAUTH_URL = googleElement.Attributes ("oauthURL").FirstOrDefault ().Value;
			GOOGLE_SUCCESS = googleElement.Attributes ("success").FirstOrDefault ().Value;
			var facebookDescendants = fbElement.Descendants ();
			var googleDescendants = googleElement.Descendants ();

#if DEBUG
			var fbDevNode = facebookDescendants.ToArray () [0];
			FACEBOOK_APP_ID = fbDevNode.Attribute ("id").Value;
			FACEBOOK_SECRET_ID = fbDevNode.Attribute ("secret").Value;
			var googleDevNode = googleDescendants.ToArray () [0];
			GOOGLE_APP_ID = googleDevNode.Attribute ("id").Value;
			GOOGLE_SECRET_ID = googleDevNode.Attribute ("secret").Value;

#else
			var fbProdNode = facebookDescendants.ToArray()[1];
			FACEBOOK_APP_ID = fbProdNode.Attribute ("id").Value;
			FACEBOOK_SECRET_ID = fbProdNode.Attribute ("secret").Value;
			var googleProdNode = googleDescendants.ToArray()[1];
			GOOGLE_APP_ID = googleProdNode.Attribute ("id").Value;
			GOOGLE_SECRET_ID = googleProdNode.Attribute ("secret").Value;

#endif
		}
	}
}
