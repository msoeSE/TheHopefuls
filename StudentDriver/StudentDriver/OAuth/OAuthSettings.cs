using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth
{
    using System;
    using System.Xml;
    using System.Diagnostics;
    using System.Xml.Linq;
    using System.Reflection;
    using System.Linq;
    namespace StudentDriver
    {
        public class OAuthSettings
        {
            private static string facebook_app_id { get; set; }
            private static string facebook_seceret_id { get; set; }
            private static string facebook_oauth_url{ get; set; }
            private static string facebook_success { get; set; }
            private static string facebook_profile_request_url { get; set; }

            public static string FACEBOOK_APP_ID
            {
                get
                {
                    if(string.IsNullOrEmpty(facebook_app_id)) InitializeKeys();
                    return facebook_app_id;
                }
            }

            public static string FACEBOOK_SECRET_ID
            {
                get
                {
                    if (string.IsNullOrEmpty(facebook_seceret_id)) InitializeKeys();
                    return facebook_seceret_id;
                }
            }
            public static string FACEBOOK_OAUTH_URL
            {
                get
                {
                    if (string.IsNullOrEmpty(facebook_oauth_url)) InitializeKeys();
                    return facebook_oauth_url;
                }
            }
            public static string FACEBOOK_SUCCESS
            {
                get
                {
                    if (string.IsNullOrEmpty(facebook_success)) InitializeKeys();
                    return facebook_success;
                }
            }
            public static string FACEBOOK_PROFILE_REQUEST_URL
            {
                get
                {
                    if (string.IsNullOrEmpty(facebook_profile_request_url)) InitializeKeys();
                    return facebook_profile_request_url;
                }
            }


            private static void InitializeKeys()
            {
                var assembly = typeof(OAuthSettings).GetTypeInfo().Assembly;
                var fileStream = assembly.GetManifestResourceStream("StudentDriver.Keys.xml");

                XDocument doc = XDocument.Load(fileStream);
                var fbElement = doc.Element("OAuth").Descendants("Facebook");
                facebook_oauth_url = fbElement.Attributes("oauthURL").FirstOrDefault().Value;
                facebook_success = fbElement.Attributes("success").FirstOrDefault().Value;
                facebook_profile_request_url = fbElement.Attributes("profileRequestUrl").FirstOrDefault().Value;
                var facebookDescendants = fbElement.Descendants();

#if DEBUG
                var fbDevNode = facebookDescendants.ToArray()[0];
                facebook_app_id = fbDevNode.Attribute("id").Value;
                facebook_seceret_id = fbDevNode.Attribute("secret").Value;
#else
			var fbProdNode = facebookDescendants.ToArray()[1];
			FACEBOOK_APP_ID = fbProdNode.Attribute ("id").Value;
			FACEBOOK_SECRET_ID = fbProdNode.Attribute ("secret").Value;
#endif
            }
        }
    }

}
