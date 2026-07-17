using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SubscriptionTypeExtensions
    {
        public static SubscriptionTypeDTO ToDTO(this SubscriptionType member)
        {
            if (member == null)
            {
                return null;
            }

            return new SubscriptionTypeDTO
            {
                Id = member.Id,
                NameEn = member.NameEn,
                NameAr = member.NameAr,
                DaysPerWeek = member.DaysPerWeek,
                DurationDays = member.DurationDays,
                Price = member.Price,
                IsActive = member.IsActive,

            };
        }

        public static SubscriptionType ToEntity(this SubscriptionTypeDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new SubscriptionType
            {
                Id = DTO.Id,
                NameEn = DTO.NameEn,
                NameAr = DTO.NameAr,
                DaysPerWeek = DTO.DaysPerWeek,
                DurationDays = DTO.DurationDays,
                Price = DTO.Price,
                IsActive = DTO.IsActive,
            };
        }

    }
}
