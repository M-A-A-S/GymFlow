using GymFlow.Domain.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.SubscriptionType
{
    public class SubscriptionTypeFilterDTO : BaseFilterDTO
    {
        public bool? IsActive { get; set; }
        public byte? MinDaysPerWeek { get; set; }
        public byte? MaxDaysPerWeek { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public short? MinDurationDays { get; set; }
        public short? MaxDurationDays { get; set; }

        // true  -> only plans with members
        // false -> only plans without members
        // null  -> all
        public bool? HasMembers { get; set; }
    }
}
