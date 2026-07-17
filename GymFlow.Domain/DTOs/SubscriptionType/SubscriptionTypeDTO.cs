using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.SubscriptionType
{
    public class SubscriptionTypeDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.NameEn),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string NameEn { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.NameAr),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string NameAr { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.DaysPerWeek),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public byte DaysPerWeek { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.DurationDays),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public short DurationDays { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Price),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public decimal Price { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.IsActive),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public bool IsActive { get; set; } = true;
    }
}
