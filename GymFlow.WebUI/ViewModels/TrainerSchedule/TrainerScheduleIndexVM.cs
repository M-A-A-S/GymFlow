using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.DTOs.TrainerSchedule;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.TrainerSchedule
{
    public class TrainerScheduleIndexVM
    {
        public PagedResult<TrainerScheduleDTO> PagedResult { get; set; }
        public TrainerScheduleFilterDTO Filter { get; set; }
        public IEnumerable<TrainerSearchDTO> Trainers { get; set; } = Enumerable.Empty<TrainerSearchDTO>();
    }
}
