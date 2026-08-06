using GymFlow.Domain.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberAttendance
{
    public class MemberAttendanceFilterDTO : BaseFilterDTO
    {
        // true = attended, false = absent, null = all
        public bool? HasAttendance { get; set; }

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
