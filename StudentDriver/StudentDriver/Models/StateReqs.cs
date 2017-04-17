using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SQLite.Net.Attributes;

namespace StudentDriver.Models
{
    [Table("StateReqs")]
    public class StateReqs
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "stateAbbreviation")]
        public string StateAbbr { get; set; }
        [JsonProperty(PropertyName = "dayHours")]
        public int DayHours { get; set; }
        [JsonProperty(PropertyName = "nightHours")]
        public int NightHours { get; set; }
        [JsonProperty(PropertyName = "inclementWeather")]
        public int InclementWeatherHours { get; set; }
        [JsonProperty(PropertyName = "nightOrInclement")]
        public bool NightOrInclement { get; set; }
    }
}
