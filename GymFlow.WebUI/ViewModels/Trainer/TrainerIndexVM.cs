using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.Trainer
{
    public class TrainerIndexVM
    {
        public PagedResult<TrainerDTO> PagedResult { get; set; }
        public TrainerFilterDTO Filter { get; set; }
    }
}
