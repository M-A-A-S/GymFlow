using GymFlow.Domain.DTOs.Common;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Member
{
    public class MemberFilterDTO : QueryRequest
    {
        public Gender? Gender { get; set; }
        public MemberStatus? Status { get; set; }
        public DateOnly? RegisterDateFrom { get; set; }
        public DateOnly? RegisterDateTo { get; set; }

    }
}
