using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum SubscriptionStatus
    {
        Active = 1,
        Inactive = 2,
        Expired = 3,
        Cancelled = 4,
        Suspended = 5,
        Unsuspended = 6
    }
}
