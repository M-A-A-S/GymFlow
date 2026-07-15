using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Entities
{
    public class MemberSubscription : BaseEntity
    {
        public int MemberId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal Price { get; set; }
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public Member Member { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
