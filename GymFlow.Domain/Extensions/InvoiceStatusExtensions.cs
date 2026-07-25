using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class InvoiceStatusExtensions
    {
        public static string ToBadgeClass(this InvoiceStatus status)
        {
            return status switch
            {
                InvoiceStatus.Draft => "bg-secondary",
                InvoiceStatus.Paid => "bg-success",
                InvoiceStatus.Partial => "bg-warning",
                InvoiceStatus.Unpaid => "bg-danger",
                InvoiceStatus.Cancelled => "bg-warning",
                _ => "bg-secondary"
            };
        }

    }
}
