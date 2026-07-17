using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberSubscription
{
    public class MemberSubscriptionDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Member),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public int MemberId { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.SubscriptionType),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public int SubscriptionTypeId { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.StartDate),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DateOnly StartDate { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.EndDate),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DateOnly EndDate { get; set; }

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
            Name = nameof(Resources.Shared.SharedResource.Status),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public MemberDTO? Member { get; set; }
        public SubscriptionTypeDTO? SubscriptionType { get; set; }
    }
}
