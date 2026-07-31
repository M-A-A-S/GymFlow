using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class AttendanceStatusExtensions
    {
        public static string ToBadgeClass(this AttendanceStatus status)
        {
            return status switch
            {
                AttendanceStatus.NotArrived => "bg-secondary",
                AttendanceStatus.Inside => "bg-success",
                AttendanceStatus.Completed => "bg-primary",
                _ => "bg-secondary"
            };
        }

        public static string GetIconClass(this AttendanceStatus status)
        {
            return status switch
            {
                AttendanceStatus.NotArrived => "fa-user-clock",
                AttendanceStatus.Inside => "fa-user-check",
                AttendanceStatus.Completed => "fa-check-circle",
                _ => "fa-user-check"
            };
        }

    }
}
