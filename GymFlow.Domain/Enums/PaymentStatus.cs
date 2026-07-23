using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = nameof(SharedResource.Unpaid), ResourceType = typeof(SharedResource))]
        Unpaid = 1,

        [Display(Name = nameof(SharedResource.Partial), ResourceType = typeof(SharedResource))]
        Partial = 2,

        [Display(Name = nameof(SharedResource.Paid), ResourceType = typeof(SharedResource))]
        Paid = 3
    }
}
