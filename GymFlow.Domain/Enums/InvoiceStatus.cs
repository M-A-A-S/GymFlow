using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum InvoiceStatus
    {
        Unpaid = 1,
        Partial = 2,
        Paid = 3,
        Cancelled = 4
    }
}
