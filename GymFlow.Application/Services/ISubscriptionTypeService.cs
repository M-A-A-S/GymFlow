using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ISubscriptionTypeService
    {
        Task<Result<int>> AddAsync(SubscriptionTypeDTO dto);
        Task<Result<bool>> UpdateAsync(int id, SubscriptionTypeDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<SubscriptionTypeDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<SubscriptionTypeDTO>>> GetAllAsync();

    }
}
