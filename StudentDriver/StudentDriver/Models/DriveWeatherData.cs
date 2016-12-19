using Newtonsoft.Json;
using SQLite;

namespace StudentDriver.Models
{
    public class DriveWeatherData
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("unsyncDriveId")]
        public int UnsyncDriveId { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("Temprature")]
        public float Temprature { get; set; }
    }
}
