using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;

namespace StudentDriver.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("driveSchoolId")]
        public int DrivingSchoolId { get; set; }

        [JsonProperty("uType")]
        public UserType UType { get; set; }

        [JsonProperty("state")]
        public StateReqs State { get; set; }

        public enum UserType
        {
            Student,
            Instructor
        }
    }

    
}
