using GymFlow.Domain.DTOs.Trainer;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ITrainerService
    {
        Task<Result<int>> AddAsync(TrainerDTO dto);
        Task<Result<bool>> UpdateAsync(int id, TrainerDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<TrainerDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<TrainerDTO>>> GetAllAsync();
        Task<Result<IEnumerable<TrainerSearchDTO>>> SearchAsync(string search);

    }
}
