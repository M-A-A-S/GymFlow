using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class MemberSubscriptionExtensions
    {
        public static MemberSubscriptionDTO ToDTO(this MemberSubscription Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new MemberSubscriptionDTO
            {
                Id = Entity.Id,
                MemberId = Entity.MemberId,
                SubscriptionTypeId = Entity.SubscriptionTypeId,
                StartDate = Entity.StartDate,
                EndDate = Entity.EndDate,
                Price = Entity.Price,
                Status = Entity.Status
            };
        }

        public static MemberSubscription ToEntity(this MemberSubscriptionDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new MemberSubscription
            {
                Id = DTO.Id,
                MemberId = DTO.MemberId,
                SubscriptionTypeId = DTO.SubscriptionTypeId,
                StartDate = DTO.StartDate,
                EndDate = DTO.EndDate,
                Price = DTO.Price,
                Status = DTO.Status
            };
        }

    }
}
