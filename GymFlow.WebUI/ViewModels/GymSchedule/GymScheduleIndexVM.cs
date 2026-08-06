using GymFlow.Domain.DTOs.GymSchedule;
using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.GymSchedule
{
    public class GymScheduleIndexVM
    {
        public PagedResult<GymScheduleDTO> PagedResult { get; set; }
        public GymScheduleFilterDTO Filter { get; set; }
    }
}
