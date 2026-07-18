using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.MemberAttendance;
using GymFlow.Domain.Entities;
using GymFlow.Domain.Utilities;
using GymFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Services
{
    public class MemberAttendanceService : IMemberAttendanceService
    {
        #region ========================= Fields & Properties =========================
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<MemberAttendanceService> _logger;
        #endregion

        #region ========================= Constructors =========================
        public MemberAttendanceService(
            IAppDbContext appDbContext,
            ILogger<MemberAttendanceService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        #endregion

        #region ========================= Get =========================
        public async Task<Result<IEnumerable<MemberAttendanceRowDTO>>> GetDailyAttendanceAsync(DateOnly date)
        {
            try
            {
                var result = await _appDbContext.Members
                .Where(x => x.MemberSubscriptions.Any(x =>
                    x.StartDate <= date &&
                    (x.EndDate == null || x.EndDate >= date)))

                .Select(x => new MemberAttendanceRowDTO
                {
                    MemberId = x.Id,
                    MemberName = x.FullName,

                    AttendanceId = x.MemberAttendances
                    .Where(x => x.AttendanceDate == date)
                    .Select(x => (int?)x.Id)
                    .FirstOrDefault(),

                    CheckIn = x.MemberAttendances
                    .Where(x => x.AttendanceDate == date)
                    .Select(x => (TimeOnly?)x.CheckIn)
                    .FirstOrDefault(),

                    CheckOut = x.MemberAttendances
                    .Where(x => x.AttendanceDate == date)
                    .Select(x => (TimeOnly?)x.CheckOut)
                    .FirstOrDefault()
                })
                .OrderBy(x => x.MemberName)
                .ToListAsync();

                return Result<IEnumerable<MemberAttendanceRowDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberService),
                    nameof(GetDailyAttendanceAsync));

                return Result<IEnumerable<MemberAttendanceRowDTO>>.Failure(
                    ResultCodes.UnexpectedError,
                    500, "An unexpected error occurred.");
            }

            
        }

        #endregion

        #region ========================= Check In =========================
        public async Task<Result<bool>> CheckInAsync(
            int memberId,
            DateOnly date
            )
        {         
            try
            {

                var exists = await _appDbContext.MemberAttendances
                    .AnyAsync(x =>
                        x.MemberId == memberId &&
                        x.AttendanceDate == date);

                if (exists)
                {
                    return Result<bool>.Failure(
                        ResultCodes.MemberAlreadyCheckedIn);
                }

                var attendance = new MemberAttendance
                {
                    MemberId = memberId,
                    AttendanceDate = date,
                    CheckIn = TimeOnly.FromDateTime(DateTime.UtcNow)
                };

                _appDbContext.MemberAttendances .Add(attendance);
                await _appDbContext.SaveChangesAsync();

                return Result<bool>.Success(
                    true,
                    ResultCodes.CheckInSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberService),
                    nameof(CheckInAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500, "An unexpected error occurred.");
            }
        }

        #endregion

        #region ========================= Check out =========================
        public async Task<Result<bool>> CheckOutAsync(int attendanceId)
        {
            try
            {
                var attendance = await _appDbContext.MemberAttendances
                    .FirstOrDefaultAsync(x => x.Id == attendanceId);

                if (attendance is null)
                {
                    return Result<bool>.Failure(ResultCodes.AttendanceNotFound);
                }

                if (attendance.CheckOut != null)
                {
                    return Result<bool>.Failure(ResultCodes.AlreadyCheckedOut);
                }

                attendance.CheckOut = TimeOnly.FromDateTime(DateTime.UtcNow);

                await _appDbContext.SaveChangesAsync();

                return Result<bool>.Success(
                    true,
                    ResultCodes.CheckOutSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Type : {Type}, Method: {Method},",
                    nameof(MemberService),
                    nameof(CheckOutAsync));

                return Result<bool>.Failure(
                    ResultCodes.UnexpectedError,
                    500, "An unexpected error occurred.");
            }
        }

        #endregion

    }
}
