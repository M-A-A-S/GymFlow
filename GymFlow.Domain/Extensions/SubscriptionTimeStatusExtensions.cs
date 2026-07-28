using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SubscriptionTimeStatusExtensions
    {
        public static string ToBadgeClass(this SubscriptionTimeStatus status)
        {
            return status switch
            {
                SubscriptionTimeStatus.Current => "bg-success",
                SubscriptionTimeStatus.Upcoming => "bg-info",
                SubscriptionTimeStatus.Expired => "bg-danger",
                _ => "bg-secondary"
            };
        }

    }
}
