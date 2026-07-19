using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SubscriptionStatusExtensions
    {
        public static string ToBadgeClass(this SubscriptionStatus status)
        {
            return status switch
            {
                SubscriptionStatus.Active => "bg-success",
                SubscriptionStatus.Inactive => "bg-danger",
                SubscriptionStatus.Suspended => "bg-danger",
                SubscriptionStatus.Unsuspended => "bg-secondary",
                SubscriptionStatus.Expired => "bg-warning",
                SubscriptionStatus.Cancelled => "bg-secondary",
                _ => "bg-secondary"
            };
        }

    }
}
