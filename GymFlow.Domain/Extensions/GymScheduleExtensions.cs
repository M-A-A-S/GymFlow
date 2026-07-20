using GymFlow.Domain.DTOs.GymSchedule;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class GymScheduleExtensions
    {
        public static GymScheduleDTO ToDTO(this GymSchedule Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new GymScheduleDTO
            {
                Id = Entity.Id,
                Day = Entity.Day,
                Gender = Entity.Gender,
                StartTime = Entity.StartTime,
                EndTime = Entity.EndTime,
            };
        }

        public static GymSchedule ToEntity(this GymScheduleDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new GymSchedule
            {
                Id = DTO.Id,
                Day = DTO.Day.Value,
                Gender = DTO.Gender.Value,
                StartTime = DTO.StartTime.Value,
                EndTime = DTO.EndTime.Value,
            };
        }

    }
}
