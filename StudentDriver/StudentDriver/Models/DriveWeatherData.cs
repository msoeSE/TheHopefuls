using SQLite.Net.Attributes;

namespace StudentDriver.Models
{
    public class DriveWeatherData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UnsyncDriveId { get; set; }
        public string WeatherType { get; set; }
        public float Temprature { get; set; }
        public string TempratureUnit { get; set; }
    }
}
