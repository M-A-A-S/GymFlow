using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class MemberAttendance : BaseEntity
    {
        public int MemberId { get; set; }
        public DateOnly AttendanceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public TimeOnly CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }

        public Member Member { get; set; }
    }
}
