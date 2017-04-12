using SQLite.Net.Attributes;

namespace StudentDriver.Models
{
    public class DriveWeatherData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UnsyncDriveId { get; set; }
        public string WeatherType { get; set; }
        public string TimeOfDay { get; set; }
    }
}
