using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberAttendance
{
    public class MemberAttendanceRowDTO
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int? AttendanceId { get; set; }
        public DateOnly AttendanceDate { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }

        public string Status
        {
            get
            {
                if (CheckIn == null)
                    return "Not Arrived";

                if (CheckOut == null)
                    return "Inside";

                return "Completed";
            }
        }
    
    }
}
