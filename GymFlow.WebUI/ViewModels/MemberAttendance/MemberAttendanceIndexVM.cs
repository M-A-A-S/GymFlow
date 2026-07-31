using GymFlow.Domain.DTOs.MemberAttendance;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.MemberAttendance
{
    public class MemberAttendanceIndexVM
    {
        public PagedResult<MemberAttendanceRowDTO> PagedResult { get; set; }
        public MemberAttendanceFilterDTO Filter { get; set; }
    }
}
