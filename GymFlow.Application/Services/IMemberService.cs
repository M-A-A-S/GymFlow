using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IMemberService
    {
        Task<Result<int>> AddAsync(MemberDTO dto);
        Task<Result<bool>> UpdateAsync(int id, MemberDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<MemberDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<MemberDTO>>> GetAllAsync();
        Task<Result<IEnumerable<MemberSearchDTO>>> SearchAsync(string search);
    }
}
