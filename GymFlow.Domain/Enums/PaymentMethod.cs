using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymFlow.Domain.Resources.Shared;

namespace GymFlow.Domain.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = nameof(SharedResource.Cash), ResourceType = typeof(SharedResource))]
        Cash = 1, // نقداً
        [Display(Name = nameof(SharedResource.Bankak), ResourceType = typeof(SharedResource))]
        Bankak = 2, // بنكك
        [Display(Name = nameof(SharedResource.Fawry), ResourceType = typeof(SharedResource))]
        Fawry = 3, // فوري
    }

}
