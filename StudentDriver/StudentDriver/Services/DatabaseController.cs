using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StudentDriver.Services
{
    public class DatabaseController
    {
        public async Task<bool> SaveUser(string profileJson)
        {
            var jsonObj = JObject.Parse(profileJson);
            var firstName = jsonObj["_json"]["first_name"].ToString();
            var lastName = jsonObj["_json"]["last_name"].ToString();
            var imgUrl = jsonObj["photos"].First["value"].ToString();
            var user = await SQLiteDatabase.GetInstance().GetUser();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.ImageUrl = imgUrl;
            return await SQLiteDatabase.GetInstance().UpdateUser(user) != -1;
        }
    }
}
