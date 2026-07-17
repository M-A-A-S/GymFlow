using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IMemberSubscriptionService
    {
        Task<Result<int>> AddAsync(MemberSubscriptionDTO dto);
        Task<Result<bool>> UpdateAsync(int id, MemberSubscriptionDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<MemberSubscriptionDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<MemberSubscriptionDTO>>> GetAllAsync();

    }
}
