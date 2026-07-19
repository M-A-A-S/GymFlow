using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class TrainerExtensions
    {
        public static TrainerDTO ToDTO(this Trainer Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new TrainerDTO
            {
                Id = Entity.Id,
                FullName = Entity.FullName,
                PhoneNumber = Entity.PhoneNumber,
                Salary = Entity.Salary,
                HireDate = Entity.HireDate,
            };
        }

        public static Trainer ToEntity(this TrainerDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new Trainer
            {
                Id = DTO.Id,
                FullName = DTO.FullName,
                PhoneNumber = DTO.PhoneNumber,
                Salary = DTO.Salary,
                HireDate = DTO.HireDate,
            };
        }

    }
}
