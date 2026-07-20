using GymFlow.Domain.DTOs.TrainerSchedule;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{

    public static class TrainerScheduleExtensions
    {
        public static TrainerScheduleDTO ToDTO(this TrainerSchedule Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new TrainerScheduleDTO
            {
                Id = Entity.Id,
                TrainerId = Entity.TrainerId,
                Day = Entity.Day,
                StartTime = Entity.StartTime,
                EndTime = Entity.EndTime,
            };
        }

        public static TrainerSchedule ToEntity(this TrainerScheduleDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new TrainerSchedule
            {
                Id = DTO.Id,
                TrainerId = DTO.TrainerId,
                Day = DTO.Day.Value,
                StartTime = DTO.StartTime.Value,
                EndTime = DTO.EndTime.Value,
            };
        }

    }
}
