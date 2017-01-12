using System;
using SQLite;

namespace StudentDriver.Models
{
    [Table("DrivePoints")]
    public class DrivePoints
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime PointDateTime { get; set; }
        public int UnsyncDriveId { get; set; }
    }
}
