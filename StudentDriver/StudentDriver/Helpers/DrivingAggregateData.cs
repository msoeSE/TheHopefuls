using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StudentDriver.Models;

namespace StudentDriver.Helpers
{
    public class DrivingAggregateData
    {
        [JsonProperty(PropertyName = "totalHours")]
        public double TotalHours { get; set; }
        [JsonProperty(PropertyName = "totalDaytimeHours")]
        public double TotalDaytimeHours { get; set; }
        [JsonProperty(PropertyName = "totalNighttimeHours")]
        public double TotalNighttimeHours { get; set; }
        [JsonProperty(PropertyName = "totalInclementHours")]
        public double TotalInclementHours { get; set; }
    }
}
