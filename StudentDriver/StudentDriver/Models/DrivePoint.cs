using System;
using Newtonsoft.Json;
using SQLite;

namespace StudentDriver.Models
{
    [Table("DrivePoints")]
    public class DrivePoint
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("unsyncDriveId")]
        public int UnsyncDriveId { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("pointDateTime")]
        public DateTime PointDateTime { get; set; }

        [JsonProperty("speed")]
        public float Speed { get; set; }
    }
}
