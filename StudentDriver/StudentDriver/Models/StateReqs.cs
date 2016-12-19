using System;
using Newtonsoft.Json;
using SQLite;

namespace StudentDriver.Models
{
    [Table("StateReqs")]
    public class StateReqs
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("totalDistanceMiles")]
        public double TotalDistanceMiles { get; set; }

        [JsonProperty("totalDuration")]
        public double TotalDuration { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
