using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDriver.Models;

namespace StudentDriver.Helpers
{
    public class DrivingDataViewModel
    {
        public DrivingDataItem Total { get; private set; }
        public DrivingDataItem TotalDayTime { get; private set; }
        public DrivingDataItem TotalNightTime { get; private set; }
        public DrivingDataItem TotalInclement { get; private set; }

        public DrivingDataViewModel(StateReqs stateReqs, DrivingAggregateData aggregateData)
        {
            var totalRequiredHours = stateReqs.DayHours + stateReqs.NightHours;
            Total = new DrivingDataItem(totalRequiredHours, aggregateData.TotalHours);
            TotalDayTime = new DrivingDataItem(stateReqs.DayHours,aggregateData.TotalDaytimeHours);
            var completedNightHours = aggregateData.TotalNighttimeHours;
            if (stateReqs.NightOrInclement)
            {
                completedNightHours += aggregateData.TotalInclementHours;
            }
            TotalNightTime = new DrivingDataItem(stateReqs.NightHours, completedNightHours);

            TotalInclement = new DrivingDataItem(stateReqs.InclementWeatherHours, aggregateData.TotalInclementHours);
        }
    }
}
