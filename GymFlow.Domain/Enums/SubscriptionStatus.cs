using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum SubscriptionStatus
    {
        [Display(Name = nameof(SharedResource.Active), ResourceType = typeof(SharedResource))]
        Active = 1,

        [Display(Name = nameof(SharedResource.Inactive), ResourceType = typeof(SharedResource))]
        Inactive = 2,

        [Display(Name = nameof(SharedResource.Expired), ResourceType = typeof(SharedResource))]
        Expired = 3,

        [Display(Name = nameof(SharedResource.Cancelled), ResourceType = typeof(SharedResource))]
        Cancelled = 4,

        [Display(Name = nameof(SharedResource.Suspended), ResourceType = typeof(SharedResource))]
        Suspended = 5,

        [Display(Name = nameof(SharedResource.Unsuspended), ResourceType = typeof(SharedResource))]
        Unsuspended = 6
    }
}
