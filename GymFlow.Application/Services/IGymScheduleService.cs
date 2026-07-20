using GymFlow.Domain.DTOs.GymSchedule;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IGymScheduleService
    {
        Task<Result<int>> AddAsync(GymScheduleDTO dto);
        Task<Result<bool>> UpdateAsync(int id, GymScheduleDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<GymScheduleDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<GymScheduleDTO>>> GetAllAsync();

    }
}
