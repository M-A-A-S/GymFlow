using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum MemberStatus
    {
        [Display(Name = nameof(SharedResource.Active), ResourceType = typeof(SharedResource))]
        Active = 1,

        [Display(Name = nameof(SharedResource.Inactive), ResourceType = typeof(SharedResource))]
        Inactive = 2,

        [Display(Name = nameof(SharedResource.Suspended), ResourceType = typeof(SharedResource))]
        Suspended = 3,

        [Display(Name = nameof(SharedResource.Unsuspended), ResourceType = typeof(SharedResource))]
        Unsuspended = 4,
    }
}
