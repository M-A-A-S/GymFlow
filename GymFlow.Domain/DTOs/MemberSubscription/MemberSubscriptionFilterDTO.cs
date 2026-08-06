using GymFlow.Domain.DTOs.Common;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberSubscription
{
    public class MemberSubscriptionFilterDTO : BaseFilterDTO
    {

        // Filter by subscription status
        public SubscriptionStatus? Status { get; set; }
        // Current / Expired / Not Started
        public SubscriptionTimeStatus? TimeStatus { get; set; }
        // Filter by member
        public int? MemberId { get; set; }

        // Filter by subscription plan
        public int? SubscriptionTypeId { get; set; }
        // Start date range
        public DateOnly? StartDateFrom { get; set; }
        public DateOnly? StartDateTo { get; set; }
        // End date range
        public DateOnly? EndDateFrom { get; set; }
        public DateOnly? EndDateTo { get; set; }
        // Price range
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        // Remaining days
        public int? MinRemainingDays { get; set; }
        public int? MaxRemainingDays { get; set; }
        // Attendance
        public int? MinAttendanceDays { get; set; }
        public int? MaxAttendanceDays { get; set; }
        // Actual Duration Days
        public short? MinDurationDays { get; set; }
        public short? MaxDurationDays { get; set; }
        // Last Attendance Date
        public DateOnly? LastAttendanceFrom { get; set; }
        public DateOnly? LastAttendanceTo { get; set; }

        // Attendance filter

        // true  -> attended at least once
        // false -> never attended
        public bool? HasAttendance { get; set; }


        // Expiring soon
        // true -> subscriptions ending soon
        public bool? ExpiringSoon { get; set; }

    }
}
