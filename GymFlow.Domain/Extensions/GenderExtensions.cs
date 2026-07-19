using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class GenderExtensions
    {
        public static string ToBadgeClass(this Gender status)
        {
            return status switch
            {
                Gender.Male => "bg-primary",
                Gender.Female => "bg-primary",
                Gender.Unknown => "bg-secondary",
                _ => "bg-secondary"
            };
        }

    }
}
