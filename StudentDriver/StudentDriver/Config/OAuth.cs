using System;
using System.Xml;
namespace StudentDriver
{
	public class OAuth
	{
		public static string FACEBOOK_APP_ID { get; private set; }
		public static string FACEBOOK_SECRET_ID { get; private set; }
		public static string GOOGLE_APP_ID { get; private set; }
		public static string GOOGLE_SECRET_ID { get; private set; }

		public OAuth ()
		{
			using (XmlReader reader = XmlReader.Create ("./Keys.xml")) {
				reader.MoveToContent ();
				while (reader.Read ()) {
					if (reader.IsStartElement ()) {
						switch (reader.Name) {
						case "Google":
							GOOGLE_APP_ID = reader ["id"];
							GOOGLE_SECRET_ID = reader ["secret"];
							break;

						case "Facebook":
							FACEBOOK_APP_ID = reader ["id"];
							FACEBOOK_SECRET_ID = reader ["secret"];
							break;
						}
					}
				}
			}
		}
	}
}
