using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum AttendanceStatus
    {
        [Display(Name = nameof(SharedResource.NotArrived), ResourceType = typeof(SharedResource))]
        NotArrived = 1,

        [Display(Name = nameof(SharedResource.Inside), ResourceType = typeof(SharedResource))]
        Inside = 2,

        [Display(Name = nameof(SharedResource.Completed), ResourceType = typeof(SharedResource))]
        Completed = 3,
    }
}
