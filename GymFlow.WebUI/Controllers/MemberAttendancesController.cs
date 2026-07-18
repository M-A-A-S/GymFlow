using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.MemberAttendance;
using GymFlow.Domain.Resources.Shared;
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

        #endregion

        #region ========================= Check In =========================
        [HttpPost]
        public async Task<IActionResult> CheckIn(int memberId)
        {
            var result = await _memberAttendanceService
                .CheckInAsync(memberId);

            if (!result.IsSuccess)
            {
                Error(result.Code);
            }

            return Json(result);
        }

        #endregion

        #region ========================= Check Out =========================

        [HttpPost]
        public async Task<IActionResult> CheckOut(int attendanceId)
        {
            var result = await _memberAttendanceService
                .CheckOutAsync(attendanceId);

            if (!result.IsSuccess)
            {
                Error(result.Code);
            }

            return Json(result.Data);
        }

        #endregion
    }
}
