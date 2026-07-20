using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.DTOs.Trainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.TrainerSchedule
{
    public class TrainerScheduleAddUpdateDTO
    {
        public TrainerScheduleDTO TrainerSchedule { get; set; } = new();
        public IEnumerable<TrainerSearchDTO> Trainers { get; set; } = Enumerable.Empty<TrainerSearchDTO>();

    }
}
