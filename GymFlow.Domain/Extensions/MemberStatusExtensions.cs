using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class MemberStatusExtensions
    {
        public static string ToBadgeClass(this MemberStatus status)
        {
            return status switch
            {
                MemberStatus.Active => "bg-success",
                MemberStatus.Suspended => "bg-danger",
                MemberStatus.Inactive => "bg-secondary",
                MemberStatus.Unsuspended => "bg-secondary",
                _ => "bg-secondary"
            };
        }

    }
}
