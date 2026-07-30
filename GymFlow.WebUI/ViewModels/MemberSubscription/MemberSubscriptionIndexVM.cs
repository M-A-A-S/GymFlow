using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.MemberSubscription
{
    public class MemberSubscriptionIndexVM
    {
        public PagedResult<MemberSubscriptionDTO> PagedResult { get; set; }
        public MemberSubscriptionFilterDTO Filter { get; set; }
        public IEnumerable<MemberSearchDTO> Members { get; set; } = Enumerable.Empty<MemberSearchDTO>();
        public IEnumerable<SubscriptionTypeSearchDTO> SubscriptionTypes { get; set; } = Enumerable.Empty<SubscriptionTypeSearchDTO>();
    }
}
