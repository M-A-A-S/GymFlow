using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Utilities
{
    public static class TimeHelper
    {
        public static decimal CalculateDurationHours(TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime < startTime)
            {
                //throw new ArgumentException("End time must be greater than or equal to start time.");
            }

            var duration = endTime - startTime;

            // Example:
            // 22:00 -> 02:00
            // -20 hours + 24 hours = 4 hours
            if (duration.TotalHours < 0)
            {
                duration += TimeSpan.FromDays(1);
            }

            //return (decimal)duration.TotalHours;
            return Math.Round(
            (decimal)duration.TotalHours,
            2);
        }

    }
}
