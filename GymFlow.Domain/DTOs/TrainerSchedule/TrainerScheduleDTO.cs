using GymFlow.Domain.DTOs.Trainer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.TrainerSchedule
{
    public class TrainerScheduleDTO
    {
        public int Id { get; set; }

        [Display(
            Name = nameof(Resources.Shared.SharedResource.Trainer),
            ResourceType = typeof(Resources.Shared.SharedResource)
        )]
        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.Required),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public int TrainerId { get; set; }

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

        public TrainerDTO? Trainer { get; set; }

        [Required(
            ErrorMessageResourceName = nameof(Resources.Shared.SharedResource.DurationHours),
            ErrorMessageResourceType = typeof(Resources.Shared.SharedResource)
        )]
        public double? DurationHours
        {
            get
            {
                if (!StartTime.HasValue || !EndTime.HasValue)
                {
                    return null;
                }

                // Calculate the difference between the end time and the start time.
                //
                // Example:
                // StartTime = 09:00
                // EndTime   = 12:30
                //
                // Calculation:
                // 12:30 - 09:00 = 03:30
                //
                // The result is a TimeSpan representing 3 hours and 30 minutes.
                var duration = EndTime.Value - StartTime.Value;

                // Check if the duration is negative.
                //
                // TimeSpan.Zero means:
                // 00:00:00
                //
                // If duration is less than zero, it means the EndTime is earlier than StartTime.
                //
                // Example:
                // StartTime = 22:00
                // EndTime   = 02:00
                //
                // Calculation:
                // 02:00 - 22:00 = -20 hours
                //
                // But this is not a negative schedule.
                // It means the schedule continues after midnight:
                //
                // 22:00 -> 24:00 = 2 hours
                // 00:00 -> 02:00 = 2 hours
                //
                // Total = 4 hours
                if (duration < TimeSpan.Zero)
                {
                    // Add 24 hours to fix the negative duration.
                    //
                    // TimeSpan.FromDays(1) = 24 hours
                    //
                    // Example:
                    // -20 hours + 24 hours = 4 hours
                    duration += TimeSpan.FromDays(1);
                }

                return duration.TotalHours;
            }
        }


    }
}
