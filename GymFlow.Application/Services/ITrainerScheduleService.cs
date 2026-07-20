using GymFlow.Domain.DTOs.TrainerSchedule;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ITrainerScheduleService
    {
        Task<Result<int>> AddAsync(TrainerScheduleDTO dto);
        Task<Result<bool>> UpdateAsync(int id, TrainerScheduleDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<TrainerScheduleDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<TrainerScheduleDTO>>> GetAllAsync();

    }
}
