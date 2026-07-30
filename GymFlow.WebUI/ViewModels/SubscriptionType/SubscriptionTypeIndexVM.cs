using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.SubscriptionType
{
    public class SubscriptionTypeIndexVM
    {
        public PagedResult<SubscriptionTypeDTO> PagedSubscriptionTypeResult { get; set; }
        public SubscriptionTypeFilterDTO Filter { get; set; }
    }
}
