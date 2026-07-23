using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class PaymentStatusExtensions
    {
        public static string ToBadgeClass(this PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Paid => "bg-success",
                PaymentStatus.Partial => "bg-warning",
                PaymentStatus.Unpaid => "bg-danger",
                _ => "bg-secondary"
            };
        }

    }
}
