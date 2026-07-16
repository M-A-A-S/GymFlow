using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class MemberExtensions
    {
        public static MemberDTO ToDTO(this Member member)
        {
            if (member == null)
            {
                return null;
            }

            return new MemberDTO
            {
                Id = member.Id,
                FullName = member.FullName,
                Gender = member.Gender,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                BirthDate = member.BirthDate,
                Address = member.Address,
                RegisterDate = member.RegisterDate,
                Status = member.Status,

            };
        }

        public static Member ToEntity(this MemberDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new Member
            {
                Id = DTO.Id,
                FullName = DTO.FullName,
                Gender = DTO.Gender,
                Email = DTO.Email,
                PhoneNumber = DTO.PhoneNumber,
                BirthDate = DTO.BirthDate,
                Address = DTO.Address,
                RegisterDate = DTO.RegisterDate,
                Status = DTO.Status ?? MemberStatus.Active
            };
        }


    }
}
