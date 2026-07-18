using GymFlow.Domain.DTOs.Member;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.MemberAttendance
{
    public class MemberAttendanceDTO
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
            Name = nameof(Resources.Shared.SharedResource.AttendanceDate),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DateOnly AttendanceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Display(
            Name = nameof(Resources.Shared.SharedResource.CheckIn),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public TimeOnly CheckIn { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.CheckOut),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        //[Required(
        //    ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
        //    ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        //)]
        public TimeOnly CheckOut { get; set; }


        public MemberDTO? Member { get; set; }
    }
}
