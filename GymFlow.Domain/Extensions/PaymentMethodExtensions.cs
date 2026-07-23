using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class PaymentMethodExtensions
    {
        public static string ToBadgeClass(this PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Cash => "bg-primary",
                PaymentMethod.Bankak => "bg-danger",
                PaymentMethod.Fawry => "bg-warning",
                _ => "bg-secondary"
            };
        }

    }
}
