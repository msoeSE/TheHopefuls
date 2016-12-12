using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDriver.Models;
using Newtonsoft.Json;


namespace StudentDriver.Helpers
{
    public class DriveSession
    {
        public UnsyncDrive UnsyncDrive { get; }
        public List<DrivePoint> DrivePoints { get; }
        public WeatherData WeatherData {get;}

        public DriveSession(UnsyncDrive unsyncDrive, List<DrivePoint> drivePoints, WeatherData weatherData)
        {
            UnsyncDrive = unsyncDrive;
            DrivePoints = drivePoints;
            WeatherData = weatherData;
        }
    }
}
