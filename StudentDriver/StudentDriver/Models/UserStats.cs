using System;
using SQLite;

namespace StudentDriver.Models
{
    [Table("UserStats")]
    public class UserStats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double TotalDistanceMiles { get; set; }
        public double TotalDurationSecs { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
