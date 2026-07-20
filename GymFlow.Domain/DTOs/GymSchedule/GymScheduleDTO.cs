using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.GymSchedule
{
    public class GymScheduleDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Day),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public DayOfWeek? Day { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Gender),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public Gender? Gender { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.StartTime),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public TimeSpan? StartTime { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.EndTime),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public TimeSpan? EndTime { get; set; }

    }
}
