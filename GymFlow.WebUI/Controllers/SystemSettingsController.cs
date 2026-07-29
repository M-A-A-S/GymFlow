using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.SystemSetting;
using GymFlow.Domain.Resources.Shared;
using GymFlow.WebUI.Extensions;
using GymFlow.WebUI.ViewModels;
using GymFlow.WebUI.ViewModels.SystemSetting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class SystemSettingsController : BaseController
    {

        #region ========================= Fields & Properties =========================
        private readonly ISystemSettingService _service;
        #endregion

        #region ========================= Constructors =========================
        public SystemSettingsController(
            ISystemSettingService systemSettingService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = systemSettingService;
        }
        #endregion


        #region ========================= Get =========================
        public async Task<IActionResult> Index()
        {

            var systemSetting = await GetEntityOrNull(_service.GetByIdAsync(1));

            if (systemSetting is null)
            {
                return NotFound();
            }

            return View(systemSetting.ToViewModel());
        }

        #endregion

        #region ========================= Update =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SystemSettingVM VM)
        {
            if (InvalidModel())
            {
                //return View(VM);
                return View(nameof(Index));
            }

            //var updateResult = await _service.UpdateAsync(VM.Id, VM.ToDTO());
            var updateResult = await _service.UpdateAsync(1, VM.ToDTO());
            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                //return View(VM);
                return View(nameof(Index));
            }

            Success(updateResult.Code);
            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
