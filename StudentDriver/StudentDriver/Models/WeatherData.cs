using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudentDriver.Helpers;

namespace StudentDriver.Models
{
    public class WeatherData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UnsyncDriveId { get; set; }
        public string WeatherType { get; set; }
        public float Temprature { get; set; }
        public string TempratureUnit { get; set; }
    }
}
