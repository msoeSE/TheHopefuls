using System;
using Newtonsoft.Json;
using SQLite;

namespace StudentDriver.Models
{
    [Table("UnsyncDrives")]
    public class UnsyncDrive
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("startDateTime")]
        public DateTime StartDateTime { get; set; }

        [JsonProperty("endDateTime")]
        public DateTime EndDateTime { get; set; }
}
}
