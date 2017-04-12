using System;
using SQLite;

namespace StudentDriver.Models
{
    [Table("StateReqs")]
    public class StateReqs
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string State { get; set; }
        public double TotalDistanceMiles { get; set; }
        public double TotalDuration { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
