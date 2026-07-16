using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class SubscriptionType : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public byte DaysPerWeek { get; set; }
        public short DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<MemberSubscription> MemberSubscriptions { get; set; }

    }
}
