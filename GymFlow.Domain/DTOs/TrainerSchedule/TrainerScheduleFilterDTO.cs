using GymFlow.Domain.DTOs.Common;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.TrainerSchedule
{
    public class TrainerScheduleFilterDTO : BaseFilterDTO
    {
        public int? TrainerId { get; set; }
        public DayOfWeek? Day { get; set; }
        public TimeSpan? StartTimeFrom { get; set; }
        public TimeSpan? StartTimeTo { get; set; }
        public decimal? MinDurationHours { get; set; }
        public decimal? MaxDurationHours { get; set; }
    }
}
