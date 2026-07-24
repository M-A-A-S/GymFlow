using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Enums
{
    public enum InvoiceStatus
    {
        // can edit freely
        Draft = 1, 

        // financial document, do not edit
        Unpaid = 2,
        Partial = 3,
        Paid = 4,

        // locked
        Cancelled = 5
    }
}
