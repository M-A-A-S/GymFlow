using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Member
{
    public class MemberDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.FullName), 
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required), 
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string FullName { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Gender),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public Gender Gender { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.BirthDate),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DateOnly BirthDate { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.PhoneNumber),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string PhoneNumber { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Email),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string Email { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Address),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public string Address { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.RegisterDate),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DateOnly RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Display(
            Name = nameof(Resources.Shared.SharedResource.MemberStatus),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public MemberStatus Status { get; set; } = MemberStatus.Active;

    }
}
