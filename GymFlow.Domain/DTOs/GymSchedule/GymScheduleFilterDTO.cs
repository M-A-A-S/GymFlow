using GymFlow.Domain.DTOs.Common;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.GymSchedule
{
    public class GymScheduleFilterDTO : BaseFilterDTO
    {
        public Gender? Gender { get; set; }
        public DayOfWeek? Day { get; set; }
        public TimeSpan? StartTimeFrom { get; set; }
        public TimeSpan? StartTimeTo { get; set; }
        public TimeSpan? EndTimeFrom { get; set; }
        public TimeSpan? EndTimeTo { get; set; }
        public decimal? MinDurationHours { get; set; }
        public decimal? MaxDurationHours { get; set; }
    }
}
