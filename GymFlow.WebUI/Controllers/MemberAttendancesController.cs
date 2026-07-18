using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.MemberAttendance;
using GymFlow.Domain.Resources.Shared;
using GymFlow.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class MemberAttendancesController : BaseController
    {
        #region ========================= Fields & Properties =========================
        private readonly IMemberAttendanceService _memberAttendanceService;
        
        #endregion

        #region ========================= Constructors =========================
        public MemberAttendancesController(
            IMemberAttendanceService memberAttendanceService,
            IStringLocalizer<SharedResource> localizer) : base (localizer)
        {
            _memberAttendanceService = memberAttendanceService;
        }

        #endregion

        #region ========================= Get =========================
        public async Task<IActionResult> Index(DateOnly? date)
        {
            var selectedDate = date
                ?? DateOnly.FromDateTime(DateTime.Today);

            var result = await _memberAttendanceService
                .GetDailyAttendanceAsync(selectedDate);

            if (!result.IsSuccess)
            {
                Error(result.Code);
                return View(new List<MemberAttendanceRowDTO>());
            }

            return View(result.Data);
        }

        public async Task<IActionResult> AttendanceTable(DateOnly date)
        {

            var result = await _memberAttendanceService
                .GetDailyAttendanceAsync(date);

            if (!result.IsSuccess)
            {
                Error(result.Code);
                return View(new List<MemberAttendanceRowDTO>());
            }

            return PartialView("Partials/_AttendanceTable", result.Data);
        }

        #endregion

        #region ========================= Check In =========================
        [HttpPost]
        public async Task<IActionResult> CheckIn(int memberId,
            DateOnly date)
        {
            var checkInResult = await _memberAttendanceService
                .CheckInAsync(memberId, date);

            var message = _localizer[checkInResult.Code].Value;
            Result<bool> result = null;

            if (!checkInResult.IsSuccess)
            {
                result = Result<bool>.Failure(checkInResult.Code, checkInResult.StatusCode, message);
                //Error(checkInResult.Code);
                return Json(result);
            }

            result = Result<bool>.Success(true, checkInResult.Code, checkInResult.StatusCode, message);

            return Json(result);
        }

        #endregion

        #region ========================= Check Out =========================

        [HttpPost]
        public async Task<IActionResult> CheckOut(int attendanceId)
        {
            var checkOutResult = await _memberAttendanceService
                .CheckOutAsync(attendanceId);

            var message = _localizer[checkOutResult.Code].Value;
            Result<bool> result = null;

            if (!checkOutResult.IsSuccess)
            {
                result = Result<bool>.Failure(checkOutResult.Code, checkOutResult.StatusCode, message);
                //Error(checkInResult.Code);
                return Json(result);
            }

            result = Result<bool>.Success(true, checkOutResult.Code, checkOutResult.StatusCode, message);

            return Json(result);
        }

        #endregion
    }
}
