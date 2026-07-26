using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum InvoiceStatus
    {
        // can edit freely
        [Display(Name = nameof(SharedResource.Draft), ResourceType = typeof(SharedResource))]
        Draft = 1,

        // financial document, do not edit

        [Display(Name = nameof(SharedResource.Unpaid), ResourceType = typeof(SharedResource))]
        Unpaid = 2,

        [Display(Name = nameof(SharedResource.Partial), ResourceType = typeof(SharedResource))]
        Partial = 3,

        [Display(Name = nameof(SharedResource.Paid), ResourceType = typeof(SharedResource))]
        Paid = 4,

        // locked
        [Display(Name = nameof(SharedResource.Cancelled), ResourceType = typeof(SharedResource))]
        Cancelled = 5
    }
}
