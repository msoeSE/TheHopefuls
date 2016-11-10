using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentDriver.Models;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StudentDriver.Services
{
    public static class WebService
    {
        private static string BaseUrl = "";

        public static async Task<User> GetStudent(int id)
        {
            var request = GenerateRequest("students",new Dictionary<string, string>(){ {"userId", id.ToString()}});
            request.ContentType = "application/json";
            request.Method = "GET";

            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var stream = response.GetResponseStream())
                {
                    var json = JObject.Parse(new StreamReader(stream).ReadToEnd());  
                    var student = JsonConvert.DeserializeObject<User>(json.ToString());
                    return student;
                }
            }

        }

        private static HttpWebRequest GenerateRequest(string endPoint, Dictionary<string,string> paramDictionary )
        {
            string requestUrl = BaseUrl + "/" + endPoint;

            string s = string.Join("&", paramDictionary.Select(x => x.Key + "=" + x.Value).ToArray());

            if (!string.IsNullOrEmpty(s))
            {
                requestUrl += "?"+s;
            }
            return (HttpWebRequest)WebRequest.Create(new Uri(requestUrl));
        }
    }
}
