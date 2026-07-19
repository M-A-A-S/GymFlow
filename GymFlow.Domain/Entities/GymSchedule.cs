using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class GymSchedule : BaseEntity
    {
        public DayOfWeek Day { get; set; }
        public Gender Gender { get; set; }   // Men or Women
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
