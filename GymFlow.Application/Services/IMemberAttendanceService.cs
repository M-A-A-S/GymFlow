using GymFlow.Domain.DTOs.MemberAttendance;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IMemberAttendanceService
    {
        Task<Result<IEnumerable<MemberAttendanceRowDTO>>> GetDailyAttendanceAsync(DateOnly date);
        Task<Result<bool>> CheckInAsync(int memberId);
        Task<Result<bool>> CheckOutAsync(int attendanceId);
    }
}
