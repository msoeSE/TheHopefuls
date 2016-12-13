using SQLite;

namespace StudentDriver.Models
{
    public class DriveWeatherData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UnsyncDriveId { get; set; }
        public string Icon { get; set; }
        public float Temprature { get; set; }
    }
}
