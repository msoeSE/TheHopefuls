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
        public static string FACEBOOK_PROFILE_REQUEST_URL { get; private set; }

		public OAuth ()
		{

		}

		public static void InitializeKeys ()
		{
			var assembly = typeof (OAuth).GetTypeInfo ().Assembly;
			var fileStream = assembly.GetManifestResourceStream ("StudentDriver.Keys.xml");

			XDocument doc = XDocument.Load (fileStream);
			var fbElement = doc.Element ("OAuth").Descendants ("Facebook");
			FACEBOOK_OAUTH_URL = fbElement.Attributes ("oauthURL").FirstOrDefault ().Value;
			FACEBOOK_SUCCESS = fbElement.Attributes ("success").FirstOrDefault ().Value;
		    FACEBOOK_PROFILE_REQUEST_URL = fbElement.Attributes("profileRequestUrl").FirstOrDefault().Value;
			var facebookDescendants = fbElement.Descendants ();

#if DEBUG
			var fbDevNode = facebookDescendants.ToArray () [0];
			FACEBOOK_APP_ID = fbDevNode.Attribute ("id").Value;
			FACEBOOK_SECRET_ID = fbDevNode.Attribute ("secret").Value;
#else
			var fbProdNode = facebookDescendants.ToArray()[1];
			FACEBOOK_APP_ID = fbProdNode.Attribute ("id").Value;
			FACEBOOK_SECRET_ID = fbProdNode.Attribute ("secret").Value;
#endif
		}
	}
}
