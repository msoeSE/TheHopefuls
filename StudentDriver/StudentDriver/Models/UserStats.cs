using System;
using Newtonsoft.Json;
using SQLite;

namespace StudentDriver.Models
{
    [Table("UserStats")]
    public class UserStats
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("totalDistanceMiles")]
        public double TotalDistanceMiles { get; set; }

        [JsonProperty("totalDurationSecs")]
        public double TotalDurationSecs { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
