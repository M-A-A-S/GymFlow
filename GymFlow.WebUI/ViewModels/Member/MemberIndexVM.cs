using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.Utilities;
using System.Reflection;

namespace GymFlow.WebUI.ViewModels.Member
{
    public class MemberIndexVM
    {
        public PagedResult<MemberDTO> Members { get; set; }
        public MemberFilterDTO Filter { get; set; }

    }
}
