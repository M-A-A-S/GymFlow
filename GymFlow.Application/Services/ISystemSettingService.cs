using GymFlow.Domain.DTOs.SystemSetting;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ISystemSettingService
    {

        Task<Result<int>> AddAsync(SystemSettingDTO dto);
        Task<Result<bool>> UpdateAsync(int id, SystemSettingDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<SystemSettingDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<SystemSettingDTO>>> GetAllAsync();
        Task<Result<SystemSettingDTO>> GetCurrentAsync();

    }
}
