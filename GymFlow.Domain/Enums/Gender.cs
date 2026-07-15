using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using GymFlow.Domain;
using GymFlow.Domain.Resources.Shared;

namespace GymFlow.Domain.Enums
{
    public enum Gender
    {
        [Display(Name = nameof(SharedResource.Male), ResourceType = typeof(SharedResource))]
        Male = 1,

        [Display(Name = nameof(SharedResource.Female), ResourceType = typeof(SharedResource))]
        Female = 2,

        [Display(Name = nameof(SharedResource.Unknown), ResourceType = typeof(SharedResource))]
        Unknown = 3
    }
}
