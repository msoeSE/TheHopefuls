using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

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
        public int DrivingSchoolId { get; set; }
        public UserType UType { get; set; }
        public StateReqs State { get; set; }

        public enum UserType
        {
            Student,
            Instructor
        }
    }

    
}
