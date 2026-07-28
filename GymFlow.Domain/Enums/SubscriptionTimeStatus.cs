using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum SubscriptionTimeStatus
    {
        [Display(Name = nameof(SharedResource.Upcoming), ResourceType = typeof(SharedResource))]
        Upcoming = 1,

        [Display(Name = nameof(SharedResource.Current), ResourceType = typeof(SharedResource))]
        Current = 2,

        [Display(Name = nameof(SharedResource.Expired), ResourceType = typeof(SharedResource))]
        Expired = 3
    }
}
