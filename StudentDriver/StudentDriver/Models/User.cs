using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SQLite.Net.Attributes;

namespace StudentDriver.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ServerId { get; set; }
        public string DrivingSchoolId { get; set; }
        public UserType UType { get; set; }
        public string ImageUrl { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public enum UserType
        {
            Student,
            Instructor
        }
    }

    
}
