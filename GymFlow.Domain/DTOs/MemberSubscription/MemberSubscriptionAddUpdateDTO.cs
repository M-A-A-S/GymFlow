using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.SubscriptionType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberSubscription
{
    public class MemberSubscriptionAddUpdateDTO
    {
        public MemberSubscriptionDTO MemberSubscription { get; set; } = new();

        public IEnumerable<MemberSearchDTO> Members { get; set; } = Enumerable.Empty<MemberSearchDTO>();
        public IEnumerable<SubscriptionTypeSearchDTO> SubscriptionTypes { get; set; } = Enumerable.Empty<SubscriptionTypeSearchDTO>();

    }
}
